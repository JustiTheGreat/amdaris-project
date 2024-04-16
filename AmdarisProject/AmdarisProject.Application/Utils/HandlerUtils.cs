using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos;
using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;

namespace AmdarisProject.Application.Utils
{
    public static class HandlerUtils
    {
        public static bool TeamContainsPlayer(Team team, Guid playerId)
            => team.Players.Any(player => player.Id.Equals(playerId));

        public static bool CompetitorIsOrIsPartOfCompetitor(Competitor competitor, Guid competitorId)
            => competitor.Id.Equals(competitorId)
            || competitor is Team team && TeamContainsPlayer(team, competitorId);

        public static bool MatchContainsCompetitor(Match match, Guid competitorId)
            => CompetitorIsOrIsPartOfCompetitor(match.CompetitorOne, competitorId)
            || CompetitorIsOrIsPartOfCompetitor(match.CompetitorTwo, competitorId);

        public static bool CompetitionContainsCompetitor(Competition competition, Guid competitorId)
            => competition.Competitors.Any(
                competitor => CompetitorIsOrIsPartOfCompetitor(competitor, competitorId));

        public static async Task<IEnumerable<RankingItemDTO>> GetCompetitionRankingUtil(IUnitOfWork unitOfWork, Guid competitionId)
        {
            Competition competition = await unitOfWork.CompetitionRepository.GetById(competitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(competitionId), competitionId));

            if (competition.Status is not CompetitionStatus.STARTED
                && competition.Status is not CompetitionStatus.FINISHED)
                throw new APIllegalStatusException(competition.Status);

            int GetCompetitorCompetitionWins(Guid competitorId)
                => competition.Matches.Where(match => match.Winner != null && match.Winner.Id.Equals(competitorId)).Count();

            int GetCompetitorCompetitionPoints(Guid competitorId)
                => competition.Matches.Where(match =>
                        match.CompetitorOne.Id.Equals(competitorId) || match.CompetitorTwo.Id.Equals(competitorId))
                    .Select(match => match.CompetitorOne.Id.Equals(competitorId)
                        ? (int)match.CompetitorOnePoints! : (int)match.CompetitorTwoPoints!)
                    .Aggregate((pointOne, pointTwo) => pointOne + pointTwo);

            IEnumerable<RankingItemDTO> ranking = [.. competition.Competitors
                .Select(competitor => new RankingItemDTO(
                    competitor.Id,
                    competitor.Name,
                    (uint)GetCompetitorCompetitionWins(competitor.Id),
                    (uint)GetCompetitorCompetitionPoints(competitor.Id)
                ))
                .OrderByDescending(entry => entry.Wins)
                    .ThenByDescending(entry => entry.Points)
                    .ThenBy(entry => entry.CompetitorName)];

            return ranking;
        }

        public static async Task<IEnumerable<Competitor>> GetCompetitionFirstPlaceCompetitorsUtil(IUnitOfWork unitOfWork, Guid competitionId)
        {
            IEnumerable<RankingItemDTO> ranking = await GetCompetitionRankingUtil(unitOfWork, competitionId);
            uint maxWinsOnCompetition = ranking.First().Wins;
            uint maxPointsOnCompetition = ranking.First().Points;
            IEnumerable<Guid> firstPlaceCompetitorIds = ranking
                .Where(rankingItem => rankingItem.Wins == maxWinsOnCompetition && rankingItem.Points == maxPointsOnCompetition)
                .Select(rankingItem => rankingItem.CompetitorId).ToList();
            IEnumerable<Competitor> firstPlaceCompetitors = await unitOfWork.CompetitorRepository.GetByIds(firstPlaceCompetitorIds);
            return firstPlaceCompetitors;
        }

        private static async Task<Match> CreateMatch(IUnitOfWork unitOfWork, IMapper mapper, string location, Guid competitorOneId,
            Guid competitorTwoId, Guid competitionId, ushort? stageLevel, ushort? stageIndex)
        {
            Competitor competitorOne = await unitOfWork.CompetitorRepository.GetById(competitorOneId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(competitorOneId), competitorOneId));

            Competitor competitorTwo = await unitOfWork.CompetitorRepository.GetById(competitorTwoId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(competitorTwoId), competitorTwoId));

            Competition competition = await unitOfWork.CompetitionRepository.GetById(competitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(competitionId), competitionId));

            if (competition.CompetitorType is CompetitorType.PLAYER
                    && (competitorOne is not Player || competitorTwo is not Player)
                || competition.CompetitorType is CompetitorType.TEAM
                    && (competitorOne is not Team || competitorTwo is not Team))
                throw new AmdarisProjectException("Competiors not matching the competition type!");

            if (competitorOne.Id.Equals(competitorTwo.Id))
                throw new AmdarisProjectException($"Trying to create a match with the same competitor on both sides!");

            if (!CompetitionContainsCompetitor(competition, competitorOne.Id))
                throw new AmdarisProjectException($"Competitor {competitorOne.Id} not registered to competition {competition.Id}!");

            if (!CompetitionContainsCompetitor(competition, competitorTwo.Id))
                throw new AmdarisProjectException($"Competitor {competitorTwo.Id} not registered to competition {competition.Id}!");

            DateTime? matchStartTime = null;

            if (competition.DurationInSeconds is not null)
            {
                if (competition.Matches.Count == 0)
                {
                    matchStartTime = competition.StartTime;
                }
                else
                {
                    DateTime lastStartTime = competition.Matches.Max(match => match.StartTime)
                        ?? throw new AmdarisProjectException("Null start time for a timed match!");

                    matchStartTime = lastStartTime.AddSeconds(
                        (double)(competition.DurationInSeconds! + competition.BreakInSeconds!));
                }
            }

            MatchCreateDTO matchCreateDTO = new()
            {
                Location = location,
                StartTime = matchStartTime,
                EndTime = null,
                Status = MatchStatus.NOT_STARTED,
                CompetitorOne = competitorOne.Id,
                CompetitorTwo = competitorTwo.Id,
                Competition = competition.Id,
                CompetitorOnePoints = null,
                CompetitorTwoPoints = null,
                Winner = null,
                StageLevel = stageLevel,
                StageIndex = stageIndex,
                Points = [],
            };

            Match mapped = mapper.Map<Match>(matchCreateDTO);
            Match created = await unitOfWork.MatchRepository.Create(mapped);
            return created;
        }

        public static async Task<IEnumerable<Match>> CreateCompetitionMatchesUtil(IUnitOfWork unitOfWork, IMapper mapper, Guid competitionId)
        {
            async Task<IEnumerable<Match>> CreateOneVSAllCompetitionMatches(OneVSAllCompetition oneVSAllCompetition, IEnumerable<Competitor> competitors)
            {
                List<Match> matches = [];

                for (int i = 0; i < competitors.Count(); i++)
                {
                    for (int j = i + 1; j < competitors.Count(); j++)
                    {
                        Match created = await CreateMatch(unitOfWork, mapper, oneVSAllCompetition.Location, competitors.ElementAt(i).Id,
                            competitors.ElementAt(j).Id, oneVSAllCompetition.Id, null, null);
                        matches.Add(created);
                    }
                }

                return matches;
            }

            async Task<IEnumerable<Match>> CreateTournamentCompetitionMatches(TournamentCompetition tournamentCompetition,
                IEnumerable<Competitor> competitors)
            {
                List<Match> matches = [];

                IEnumerable<Competitor> competitorsByRating = [.. competitors
                    .OrderByDescending(competitor =>
                        unitOfWork.MatchRepository
                            .GetCompetitorWinRatingForGameType(competitor.Id, tournamentCompetition.GameType).Result)];

                //TODO tournament competition cancel advancement logic

                for (int i = 0; i < competitors.Count(); i += 2)
                {
                    Match created = await CreateMatch(unitOfWork, mapper, tournamentCompetition.Location, competitors.ElementAt(i).Id,
                        competitors.ElementAt(i + 1).Id, tournamentCompetition.Id,
                        (ushort?)(competitors.Count() / 2)/*TODO tournamentCompetition.StageLevel*/, (ushort?)i);
                    matches.Add(created);
                }

                return matches;
            }

            IEnumerable<Match> createdMatches = [];

            Competition competition = await unitOfWork.CompetitionRepository.GetById(competitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(competitionId), competitionId));

            bool allMatchesWerePlayed = !(await unitOfWork.MatchRepository.GetUnfinishedByCompetition(competition.Id)).Any();

            if (!allMatchesWerePlayed)
                return createdMatches;

            IEnumerable<Competitor> firstPlaceCompetitors = await GetCompetitionFirstPlaceCompetitorsUtil(unitOfWork, competition.Id);

            if (firstPlaceCompetitors.Count() <= 1)
                return createdMatches;

            if (competition is OneVSAllCompetition oneVSAllCompetition)
                createdMatches = await CreateOneVSAllCompetitionMatches(oneVSAllCompetition, firstPlaceCompetitors);
            else if (competition is TournamentCompetition tournamentCompetition)
                //TODO it's not right to use first place competitors
                createdMatches = await CreateTournamentCompetitionMatches(tournamentCompetition, firstPlaceCompetitors);

            return createdMatches;
        }
    }
}
