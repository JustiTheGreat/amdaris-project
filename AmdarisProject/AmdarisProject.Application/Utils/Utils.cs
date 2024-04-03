using AmdarisProject.dtos;
using AmdarisProject.handlers.competition;
using AmdarisProject.handlers.competitor;
using AmdarisProject.models;
using AmdarisProject.models.competition;
using AmdarisProject.models.competitor;
using AmdarisProject.repositories.abstractions;
using Domain.Enums;
using Domain.Exceptions;
using System.Reflection.Metadata;

namespace AmdarisProject.utils
{
    public static class Utils
    {
        public static bool CompetitionContainsCompetitor(Competition competition, ulong competitorId)
            => competition.Competitors.Any(competitor =>
                competitor.Id == competitorId
                    || competition.CompetitorType is CompetitorType.TEAM
                        && TeamContainsPlayer((Team)competitor, competitorId));

        public static bool MatchContainsCompetitor(Match match, ulong competitorId)
            => match.CompetitorOne.Id == competitorId
            || match.CompetitorTwo.Id == competitorId
            || match.Competition.CompetitorType is CompetitorType.TEAM
                && TeamContainsPlayer((Team)match.CompetitorOne, competitorId)
                    || TeamContainsPlayer((Team)match.CompetitorTwo, competitorId);

        public static bool TeamContainsPlayer(Team team, ulong playerId)
            => team.Players.Any(player => player.Id == playerId);

        public static uint GetCompetitorMatchPointsUtil(IMatchRepository matchRepository, ICompetitorRepository competitorRepository,
            IPointRepository pointRepository, ulong matchId, ulong competitorId)
        {
            Match match = matchRepository.GetById(matchId);

            if (match.Status is not MatchStatus.STARTED
                && match.Status is not MatchStatus.FINISHED
                && match.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                && match.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                throw new APIllegalStatusException(nameof(Utils), nameof(GetCompetitorMatchPointsUtil), match.Status.ToString());

            if (!matchRepository.ContainsCompetitor(matchId, competitorId))
                throw new APCompetitorException(nameof(Utils), nameof(GetCompetitorMatchPointsUtil),
                    $"Competitor {competitorId} not in match!");

            Competitor competitor = competitorRepository.GetById(competitorId);
            uint points = competitor is Player
                ? pointRepository.GetByPlayerAndMatch(competitorId, matchId).Value
                : (competitor as Team)!.Players
                    .Select(player => pointRepository.GetByPlayerAndMatch(player.Id, matchId).Value)
                    .Aggregate((point1, point2) => point1 + point2);
            return points;
        }

        public static bool MatchHasACompetitorWithTheWinningScoreUtil(IMatchRepository matchRepository,
            ICompetitorRepository competitorRepository, IPointRepository pointRepository, ulong matchId)
        {
            Match match = matchRepository.GetById(matchId);

            if (match.Status is not MatchStatus.STARTED or MatchStatus.FINISHED)
                throw new APIllegalStatusException(nameof(Utils), nameof(MatchHasACompetitorWithTheWinningScoreUtil), match.Status.ToString());

            uint? winAt = match.Competition.WinAt;
            bool aCompetitorHasTheWinningScore = winAt.HasValue
                && (GetCompetitorMatchPointsUtil(matchRepository, competitorRepository, pointRepository,
                        match.Id, match.CompetitorOne.Id) == winAt
                    || GetCompetitorMatchPointsUtil(matchRepository, competitorRepository, pointRepository,
                        match.Id, match.CompetitorTwo.Id) == winAt);
            return aCompetitorHasTheWinningScore;
        }

        public static Competitor? GetMatchWinnerUtil(IMatchRepository matchRepository,
            ICompetitorRepository competitorRepository, IPointRepository pointRepository, ulong matchId)
        {
            Match match = matchRepository.GetById(matchId);

            if (match.Status is MatchStatus.NOT_STARTED or MatchStatus.STARTED or MatchStatus.CANCELED)
                throw new APIllegalStatusException(nameof(Utils), nameof(GetMatchWinnerUtil), match.Status.ToString());

            Competitor? winner = null;

            if (match.Status is MatchStatus.SPECIAL_WIN_COMPETITOR_ONE)
                winner = match.CompetitorOne;
            else if (match.Status is MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                winner = match.CompetitorTwo;
            else
            {
                uint pointsCompetitorOne = GetCompetitorMatchPointsUtil(matchRepository, competitorRepository, pointRepository,
                    match.Id, match.CompetitorOne.Id);
                uint pointsCompetitorTwo = GetCompetitorMatchPointsUtil(matchRepository, competitorRepository, pointRepository,
                    match.Id, match.CompetitorTwo.Id);
                winner = pointsCompetitorOne == pointsCompetitorTwo ? null
                    : pointsCompetitorOne > pointsCompetitorTwo ? match.CompetitorOne
                    : match.CompetitorTwo;
            }

            return winner;
        }

        public static bool CompetitorIsPartOfTheWinningSideUtil(IMatchRepository matchRepository, ICompetitorRepository competitorRepository,
            IPointRepository pointRepository, ulong matchId, ulong competitorId)
        {
            Competitor? competitor = GetMatchWinnerUtil(matchRepository, competitorRepository, pointRepository, matchId);
            return competitor?.Id == competitorId || competitor is Team team && TeamContainsPlayer(team, competitor.Id);
        }

        public static double GetCompetitorWinRatingOfMatchesUtil(IMatchRepository matchRepository, ICompetitorRepository competitorRepository,
            IPointRepository pointRepository, IEnumerable<Match> playedMatches, ulong competitorId)
        {
            uint wins = (uint)playedMatches.Count(match => CompetitorIsPartOfTheWinningSideUtil(matchRepository, competitorRepository,
                    pointRepository, match.Id, competitorId));
            double rating = (double)wins / playedMatches.Count();
            return rating;
        }

        public static uint GetCompetitorCompetitionWinsUtil(IMatchRepository matchRepository, ICompetitorRepository competitorRepository,
            IPointRepository pointRepository, ICompetitionRepository competitionRepository, ulong competitorId, ulong competitionId)
        {
            Competition competition = competitionRepository.GetById(competitionId);

            if (competition.Status is not CompetitionStatus.STARTED
                && competition.Status is not CompetitionStatus.FINISHED)
                throw new APIllegalStatusException(nameof(GetCompetitorCompetitionPointsHandler), nameof(Handle), competition.Status.ToString());

            uint wins = (uint)matchRepository.GetAllByCompetitorAndCompetition(competitorId, competitionId)
                .Count(match => CompetitorIsPartOfTheWinningSideUtil(matchRepository, competitorRepository,
                    pointRepository, match.Id, competitorId));
            return wins;
        }

        public static uint GetCompetitorCompetitionPointsUtil(IMatchRepository matchRepository, ICompetitorRepository competitorRepository,
            IPointRepository pointRepository, ICompetitionRepository competitionRepository, ulong competitorId, ulong competitionId)
        {
            Competition competition = competitionRepository.GetById(competitionId);

            if (competition.Status is not CompetitionStatus.STARTED
                && competition.Status is not CompetitionStatus.FINISHED)
                throw new APIllegalStatusException(nameof(GetCompetitorCompetitionPointsHandler), nameof(Handle), competition.Status.ToString());

            uint points = matchRepository.GetAllByCompetitorAndCompetition(competitorId, competitionId)
                .Select(match => GetCompetitorMatchPointsUtil(matchRepository, competitorRepository, pointRepository,
                    match.Id, competitorId))
                .Aggregate((points1, points2) => points1 + points2);
            return points;
        }

        public static IEnumerable<RankingItem> GetCompetitionRankingUtil(ICompetitionRepository competitionRepository,
            IMatchRepository matchRepository, ICompetitorRepository competitorRepository, IPointRepository pointRepository, ulong competitionId)
        {
            Competition competition = competitionRepository.GetById(competitionId);

            if (competition.Status is not CompetitionStatus.STARTED
                && competition.Status is not CompetitionStatus.FINISHED)
                throw new APIllegalStatusException(nameof(GetCompetitionRankingHandler), nameof(Handle), competition.Status.ToString());

            IEnumerable<RankingItem> ranking = competition.Competitors
                .Select(competitor => new RankingItem(
                    competitor.Id,
                    competitor.Name,
                    GetCompetitorCompetitionWinsUtil(matchRepository, competitorRepository, pointRepository, competitionRepository,
                        competitor.Id, competition.Id),
                    GetCompetitorCompetitionPointsUtil(matchRepository, competitorRepository, pointRepository, competitionRepository,
                        competitor.Id, competition.Id)
                ))
                .OrderByDescending(entry => entry.Wins)
                    .ThenByDescending(entry => entry.Points)
                    .ThenBy(entry => entry.CompetitorName)
                    .ToList();

            return ranking;
        }

        public static IEnumerable<Competitor> GetCompetitionFirstPlaceCompetitorsUtil(ICompetitionRepository competitionRepository,
            IMatchRepository matchRepository, ICompetitorRepository competitorRepository, IPointRepository pointRepository, ulong competitionId)
        {
            IEnumerable<RankingItem> ranking = GetCompetitionRankingUtil(competitionRepository, matchRepository, competitorRepository,
                pointRepository, competitionId);
            uint maxWinsOnCompetition = ranking.First().Wins;
            uint maxPointsOnCompetition = ranking.First().Points;
            IEnumerable<ulong> firstPlaceCompetitorIds = ranking
                .Where(rankingItem => rankingItem.Wins == maxWinsOnCompetition && rankingItem.Points == maxPointsOnCompetition)
                .Select(rankingItem => rankingItem.CompetitorId).ToList();
            IEnumerable<Competitor> firstPlaceCompetitors = competitorRepository.GetByIds(firstPlaceCompetitorIds);
            return firstPlaceCompetitors;
        }

        public static Match CreateMatch(IMatchRepository matchRepository, ICompetitorRepository competitorRepository,
            ICompetitionRepository competitionRepository, string location, ulong competitorOneId, ulong competitorTwoId, ulong competitionId)
        {
            Competitor competitorOne = competitorRepository.GetById(competitorOneId);
            Competitor competitorTwo = competitorRepository.GetById(competitorTwoId);
            Competition competition = competitionRepository.GetById(competitionId);

            if (competition.CompetitorType is CompetitorType.PLAYER
                    && (competitorOne is not Player || competitorTwo is not Player)
                || competition.CompetitorType is CompetitorType.TEAM
                    && (competitorOne is not Team || competitorTwo is not Team))
                throw new APCompetitorException(nameof(Utils), nameof(CreateMatch), "Competiors not matching the competition type!");

            if (competitorOne.Id == competitorTwo.Id)
                throw new APCompetitorException(nameof(Utils), nameof(CreateMatch),
                    $"Trying to create a match with the same competitor on both sides!");

            if (!CompetitionContainsCompetitor(competition, competitorOne.Id))
                throw new APCompetitorException(nameof(Utils), nameof(CreateMatch),
                    $"Competitor {competitorOne.Id} not registered to competition {competition.Id}!");

            if (!CompetitionContainsCompetitor(competition, competitorTwo.Id))
                throw new APCompetitorException(nameof(Utils), nameof(CreateMatch),
                $"Competitor {competitorTwo.Id} not registered to competition {competition.Id}!");

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
                        ?? throw new AmdarisProjectException(nameof(Utils), nameof(CreateMatch), "Null start time for a timed match!");

                    matchStartTime = lastStartTime.AddSeconds(
                        (double)(competition.DurationInSeconds! + competition.BreakInSeconds!));
                }
            }

            Match match = new(location, matchStartTime, null, MatchStatus.NOT_STARTED, competitorOne, competitorTwo, competition, null, []);
            Match created = matchRepository.Create(match);
            return created;
        }

        public static Stage CreateStage(IMatchRepository matchRepository, ICompetitionRepository competitionRepository,
            IStageRepository stageRepository, ulong competitionId, IEnumerable<ulong> matchIds)
        {
            IEnumerable<Match> matches = matchRepository.GetByIds(matchIds);

            if (!matches.Any())
                throw new APArgumentException(nameof(Utils), nameof(CreateStage), nameof(matches));

            int numberOfMatches = matches.Count();
            ushort stageLevel = 0;

            while (numberOfMatches != 1)
            {
                if (numberOfMatches % 2 == 1)
                    throw new APArgumentException(nameof(Utils), nameof(CreateStage), nameof(matches));

                stageLevel++;
                numberOfMatches /= numberOfMatches;
            }

            Competition competition = competitionRepository.GetById(competitionId);

            foreach (Match match in matches)
            {
                if (match.Id != competition.Id || match.Status is not MatchStatus.NOT_STARTED)
                    throw new APArgumentException(nameof(Utils), nameof(CreateStage), nameof(matches));
            }

            Stage stage = new(stageLevel, matches.ToList(), competition);
            Stage created = stageRepository.Create(stage);
            return created;
        }

        public static void CheckCompetitionCompetitorNumber(Competition competition)
        {
            void CheckOneVSAllCompetitionCompetitorNumber(OneVSAllCompetition oneVSAllCompetition)
            {
                int competitorNumber = oneVSAllCompetition.Competitors.Count;

                if (competitorNumber < 2)
                    throw new APCompetitorNumberException(nameof(OneVSAllCompetition), nameof(CheckOneVSAllCompetitionCompetitorNumber), competitorNumber.ToString());
            }

            void CheckTournamentCompetitionCompetitorNumber(TournamentCompetition tournamentCompetition)
            {
                int competitorNumber = tournamentCompetition.Competitors.Count;

                if (competitorNumber < 2)
                    throw new APCompetitorNumberException(nameof(Utils), nameof(CheckCompetitionCompetitorNumber), competitorNumber.ToString());

                while (competitorNumber != 1)
                {
                    if (competitorNumber % 2 == 1)
                        throw new APCompetitorNumberException(nameof(Utils), nameof(CheckCompetitionCompetitorNumber), competitorNumber.ToString());
                    competitorNumber /= 2;
                }
            }

            if (competition is OneVSAllCompetition oneVSAllCompetition)
                CheckOneVSAllCompetitionCompetitorNumber(oneVSAllCompetition);
            else if (competition is TournamentCompetition tournamentCompetition)
                CheckTournamentCompetitionCompetitorNumber(tournamentCompetition);
        }

        public static IEnumerable<Match> CreateCompetitionMatchesUtil(ICompetitionRepository competitionRepository, IMatchRepository matchRepository,
            ICompetitorRepository competitorRepository, IPointRepository pointRepository, IStageRepository stageRepository, ulong competitionId)
        {
            IEnumerable<Match> CreateOneVSAllCompetitionMatches(OneVSAllCompetition oneVSAllCompetition,
                IEnumerable<Competitor> competitors)
            {
                List<Match> matches = [];

                for (int i = 0; i < competitors.Count(); i++)
                {
                    for (int j = i + 1; j < competitors.Count(); j++)
                    {
                        Match created = CreateMatch(matchRepository, competitorRepository, competitionRepository,
                            oneVSAllCompetition.Location, competitors.ElementAt(i).Id, competitors.ElementAt(j).Id, oneVSAllCompetition.Id);
                        matches.Add(created);
                    }
                }

                return matches;
            }

            IEnumerable<Match> CreateTournamentCompetitionMatches(TournamentCompetition tournamentCompetition,
                IEnumerable<Competitor> competitors)
            {
                List<Match> matches = [];

                IEnumerable<Competitor> competitorsByRating = competitors
                    .OrderByDescending(competitor =>
                    {
                        IEnumerable<Match> playedMatches = matchRepository.GetAllByCompetitorAndGameType(competitor.Id,
                            tournamentCompetition.GameType);
                        double rating = GetCompetitorWinRatingOfMatchesUtil(matchRepository, competitorRepository, pointRepository,
                            playedMatches, tournamentCompetition.Id);
                        return rating;
                    })
                .ToList();

                //TODO tournament competition cancel advancement logic

                List<ulong> stageMatchIds = [];
                for (int i = 0; i < competitors.Count(); i += 2)
                {
                    Match created = CreateMatch(matchRepository, competitorRepository, competitionRepository,
                        tournamentCompetition.Location, competitors.ElementAt(i).Id, competitors.ElementAt(i + 1).Id,
                        tournamentCompetition.Id);
                    stageMatchIds.Add(created.Id);
                    matches.Add(created);
                }
                Stage stage = CreateStage(matchRepository, competitionRepository, stageRepository, tournamentCompetition.Id, stageMatchIds);
                return matches;
            }

            Competition competition = competitionRepository.GetById(competitionId);
            bool allMatchesWerePlayed = !matchRepository.GetUnfinishedByCompetition(competition.Id).Any();

            if (!allMatchesWerePlayed)
                return [];

            IEnumerable<Competitor> firstPlaceCompetitors = GetCompetitionFirstPlaceCompetitorsUtil(competitionRepository,
                matchRepository, competitorRepository, pointRepository, competition.Id);

            if (firstPlaceCompetitors.Count() <= 1)
                return [];

            IEnumerable<Match> createdMatches = [];

            if (competition is OneVSAllCompetition oneVSAllCompetition)
                createdMatches = CreateOneVSAllCompetitionMatches(oneVSAllCompetition, firstPlaceCompetitors);
            else if (competition is TournamentCompetition tournamentCompetition)
                createdMatches = CreateTournamentCompetitionMatches(tournamentCompetition, firstPlaceCompetitors);

            return createdMatches;
        }
    }
}
