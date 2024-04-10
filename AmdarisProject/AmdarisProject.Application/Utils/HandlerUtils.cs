﻿using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos;
using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Utils.ExtensionMethods;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;

namespace AmdarisProject.Application.Utils
{
    public static class HandlerUtils
    {
        public static bool CompetitionContainsCompetitor(Competition competition, ulong competitorId)
            => competition.Competitors.Any(
                competitor => competitor.Id == competitorId
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

        public static async Task<uint> GetCompetitorMatchPointsUtil(IUnitOfWork unitOfWork, ulong matchId, ulong competitorId)
        {
            Match match = await unitOfWork.MatchRepository.GetById(matchId)
                ?? throw new APNotFoundException(nameof(HandlerUtils), nameof(GetCompetitorMatchPointsUtil),
                    [Tuple.Create(nameof(matchId), matchId)]);

            if (match.Status is not MatchStatus.STARTED
                && match.Status is not MatchStatus.FINISHED
                && match.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                && match.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                throw new APIllegalStatusException(nameof(HandlerUtils), nameof(GetCompetitorMatchPointsUtil), match.Status);

            if (!MatchContainsCompetitor(match, competitorId))
                throw new APCompetitorException(nameof(HandlerUtils), nameof(GetCompetitorMatchPointsUtil),
                    $"Competitor {competitorId} not in match!");

            Competitor competitor = await unitOfWork.CompetitorRepository.GetById(competitorId)
                ?? throw new APNotFoundException(nameof(HandlerUtils), nameof(GetCompetitorMatchPointsUtil),
                    [Tuple.Create(nameof(competitorId), competitorId)]);

            uint points = competitor is Player
                ? unitOfWork.PointRepository.GetByPlayerAndMatch(competitorId, matchId).Result?.Value
                    ?? throw new APNotFoundException(nameof(HandlerUtils), nameof(GetCompetitorMatchPointsUtil),
                        [Tuple.Create(nameof(competitorId), competitorId), Tuple.Create(nameof(matchId), matchId)])
                : ((Team)competitor).Players
                    .Select(player => unitOfWork.PointRepository.GetByPlayerAndMatch(player.Id, matchId).Result?.Value
                        ?? throw new APNotFoundException(nameof(HandlerUtils), nameof(GetCompetitorMatchPointsUtil),
                            [Tuple.Create(nameof(player.Id), player.Id), Tuple.Create(nameof(matchId), matchId)]))
                    .Aggregate((point1, point2) => point1 + point2);

            return points;
        }

        public static async Task<bool> MatchHasACompetitorWithTheWinningScoreUtil(IUnitOfWork unitOfWork, ulong matchId)
        {
            Match match = await unitOfWork.MatchRepository.GetById(matchId)
                ?? throw new APNotFoundException(nameof(HandlerUtils), nameof(MatchHasACompetitorWithTheWinningScoreUtil),
                    [Tuple.Create(nameof(matchId), matchId)]);

            if (match.Status is not MatchStatus.STARTED or MatchStatus.FINISHED)
                throw new APIllegalStatusException(nameof(HandlerUtils), nameof(MatchHasACompetitorWithTheWinningScoreUtil), match.Status);

            uint? winAt = match.Competition.WinAt;
            bool aCompetitorHasTheWinningScore = winAt.HasValue
                && (GetCompetitorMatchPointsUtil(unitOfWork, match.Id, match.CompetitorOne.Id).Result == winAt
                    || GetCompetitorMatchPointsUtil(unitOfWork, match.Id, match.CompetitorTwo.Id).Result == winAt);
            return aCompetitorHasTheWinningScore;
        }

        private static async Task<Competitor?> GetMatchWinnerUtil(IUnitOfWork unitOfWork, ulong matchId)
        {
            Match match = await unitOfWork.MatchRepository.GetById(matchId)
                ?? throw new APNotFoundException(nameof(HandlerUtils), nameof(GetMatchWinnerUtil),
                [Tuple.Create(nameof(matchId), matchId)]);

            if (match.Status is MatchStatus.NOT_STARTED or MatchStatus.STARTED or MatchStatus.CANCELED)
                throw new APIllegalStatusException(nameof(HandlerUtils), nameof(GetMatchWinnerUtil), match.Status);

            Competitor? winner = null;

            if (match.Status is MatchStatus.SPECIAL_WIN_COMPETITOR_ONE)
                winner = match.CompetitorOne;
            else if (match.Status is MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                winner = match.CompetitorTwo;
            else
            {
                uint pointsCompetitorOne = await GetCompetitorMatchPointsUtil(unitOfWork, match.Id, match.CompetitorOne.Id);
                uint pointsCompetitorTwo = await GetCompetitorMatchPointsUtil(unitOfWork, match.Id, match.CompetitorTwo.Id);
                winner = pointsCompetitorOne > pointsCompetitorTwo ? match.CompetitorOne
                    : pointsCompetitorOne > pointsCompetitorTwo ? match.CompetitorTwo
                    : null;
            }

            return winner;
        }

        public static async Task<bool> CompetitorIsPartOfTheWinningSideUtil(IUnitOfWork unitOfWork, ulong matchId, ulong competitorId)
        {
            Competitor? competitor = await GetMatchWinnerUtil(unitOfWork, matchId);
            return competitor is not null && competitor.Id == competitorId
                || competitor is Team team && TeamContainsPlayer(team, competitor.Id);
        }

        public static double GetCompetitorWinRatingOfMatchesUtil(IUnitOfWork unitOfWork, IEnumerable<Match> playedMatches, ulong competitorId)
        {
            uint wins = (uint)playedMatches.Count(match => CompetitorIsPartOfTheWinningSideUtil(unitOfWork, match.Id, competitorId).Result);
            double rating = (double)wins / playedMatches.Count();
            return rating;
        }

        public static async Task<uint> GetCompetitorCompetitionWinsUtil(IUnitOfWork unitOfWork, ulong competitorId, ulong competitionId)
        {
            Competition competition = await unitOfWork.CompetitionRepository.GetById(competitionId)
                ?? throw new APNotFoundException(nameof(HandlerUtils), nameof(GetCompetitorCompetitionWinsUtil),
                [Tuple.Create(nameof(competitionId), competitionId)]);

            if (competition.Status is not CompetitionStatus.STARTED
                && competition.Status is not CompetitionStatus.FINISHED)
                throw new APIllegalStatusException(nameof(HandlerUtils), nameof(GetCompetitorCompetitionWinsUtil), competition.Status);

            uint wins = (uint)unitOfWork.MatchRepository.GetAllByCompetitorAndCompetition(competitorId, competitionId).Result
                .Count(match => CompetitorIsPartOfTheWinningSideUtil(unitOfWork, match.Id, competitorId).Result);
            return wins;
        }

        public static async Task<uint> GetCompetitorCompetitionPointsUtil(IUnitOfWork unitOfWork, ulong competitorId, ulong competitionId)
        {
            Competition competition = await unitOfWork.CompetitionRepository.GetById(competitionId)
                ?? throw new APNotFoundException(nameof(HandlerUtils), nameof(GetCompetitorCompetitionPointsUtil),
                    [Tuple.Create(nameof(competitionId), competitionId)]);

            if (competition.Status is not CompetitionStatus.STARTED
                && competition.Status is not CompetitionStatus.FINISHED)
                throw new APIllegalStatusException(nameof(HandlerUtils), nameof(GetCompetitorCompetitionPointsUtil), competition.Status);

            uint points = unitOfWork.MatchRepository.GetAllByCompetitorAndCompetition(competitorId, competitionId).Result
                .Select(match => GetCompetitorMatchPointsUtil(unitOfWork, match.Id, competitorId).Result)
                .Aggregate((points1, points2) => points1 + points2);
            return points;
        }

        public static async Task<IEnumerable<RankingItemDTO>> GetCompetitionRankingUtil(IUnitOfWork unitOfWork, ulong competitionId)
        {
            Competition competition = await unitOfWork.CompetitionRepository.GetById(competitionId)
                ?? throw new APNotFoundException(nameof(HandlerUtils), nameof(GetCompetitionRankingUtil),
                    [Tuple.Create(nameof(competitionId), competitionId)]);

            if (competition.Status is not CompetitionStatus.STARTED
                && competition.Status is not CompetitionStatus.FINISHED)
                throw new APIllegalStatusException(nameof(HandlerUtils), nameof(GetCompetitionRankingUtil), competition.Status);

            IEnumerable<RankingItemDTO> ranking = [.. competition.Competitors
                .Select(competitor => new RankingItemDTO(
                    competitor.Id,
                    competitor.Name,
                    GetCompetitorCompetitionWinsUtil(unitOfWork, competitor.Id, competition.Id).Result,
                    GetCompetitorCompetitionPointsUtil(unitOfWork, competitor.Id, competition.Id).Result
                ))
                .OrderByDescending(entry => entry.Wins)
                    .ThenByDescending(entry => entry.Points)
                    .ThenBy(entry => entry.CompetitorName)];

            return ranking;
        }

        public static async Task<IEnumerable<Competitor>> GetCompetitionFirstPlaceCompetitorsUtil(IUnitOfWork unitOfWork, ulong competitionId)
        {
            IEnumerable<RankingItemDTO> ranking = await GetCompetitionRankingUtil(unitOfWork, competitionId);
            uint maxWinsOnCompetition = ranking.First().Wins;
            uint maxPointsOnCompetition = ranking.First().Points;
            IEnumerable<ulong> firstPlaceCompetitorIds = ranking
                .Where(rankingItem => rankingItem.Wins == maxWinsOnCompetition && rankingItem.Points == maxPointsOnCompetition)
                .Select(rankingItem => rankingItem.CompetitorId).ToList();
            IEnumerable<Competitor> firstPlaceCompetitors = await unitOfWork.CompetitorRepository.GetByIds(firstPlaceCompetitorIds);
            return firstPlaceCompetitors;
        }

        private static async Task<Match> CreateMatch(IUnitOfWork unitOfWork, string location, ulong competitorOneId,
            ulong competitorTwoId, ulong competitionId)
        {
            Competitor competitorOne = await unitOfWork.CompetitorRepository.GetById(competitorOneId)
                ?? throw new APNotFoundException(nameof(HandlerUtils), nameof(CreateMatch),
                    [Tuple.Create(nameof(competitorOneId), competitorOneId)]);

            Competitor competitorTwo = await unitOfWork.CompetitorRepository.GetById(competitorTwoId)
                ?? throw new APNotFoundException(nameof(HandlerUtils), nameof(CreateMatch),
                    [Tuple.Create(nameof(competitorTwoId), competitorTwoId)]);

            Competition competition = await unitOfWork.CompetitionRepository.GetById(competitionId)
                ?? throw new APNotFoundException(nameof(HandlerUtils), nameof(CreateMatch),
                    [Tuple.Create(nameof(competitionId), competitionId)]);

            if (competition.CompetitorType is CompetitorType.PLAYER
                    && (competitorOne is not Player || competitorTwo is not Player)
                || competition.CompetitorType is CompetitorType.TEAM
                    && (competitorOne is not Team || competitorTwo is not Team))
                throw new APCompetitorException(nameof(HandlerUtils), nameof(CreateMatch), "Competiors not matching the competition type!");

            if (competitorOne.Id == competitorTwo.Id)
                throw new APCompetitorException(nameof(HandlerUtils), nameof(CreateMatch),
                    $"Trying to create a match with the same competitor on both sides!");

            if (!CompetitionContainsCompetitor(competition, competitorOne.Id))
                throw new APCompetitorException(nameof(HandlerUtils), nameof(CreateMatch),
                    $"Competitor {competitorOne.Id} not registered to competition {competition.Id}!");

            if (!CompetitionContainsCompetitor(competition, competitorTwo.Id))
                throw new APCompetitorException(nameof(HandlerUtils), nameof(CreateMatch),
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
                        ?? throw new AmdarisProjectException(nameof(HandlerUtils), nameof(CreateMatch), "Null start time for a timed match!");

                    matchStartTime = lastStartTime.AddSeconds(
                        (double)(competition.DurationInSeconds! + competition.BreakInSeconds!));
                }
            }

            MatchCreateDTO match = new()
            {
                Location = location,
                StartTime = matchStartTime,
                EndTime = null,
                Status = MatchStatus.NOT_STARTED,
                CompetitorOne = competitorOne.Id,
                CompetitorTwo = competitorTwo.Id,
                Competition = competition.Id,
                Stage = null,
                Points = [],
            };

            Match created = await unitOfWork.MatchRepository.Create(match.Adapt<Match>());
            return created;
        }

        private static async Task<Stage> CreateStage(IUnitOfWork unitOfWork, ulong competitionId, IEnumerable<ulong> matchIds)
        {
            IEnumerable<Match> matches = await unitOfWork.MatchRepository.GetByIds(matchIds);

            if (!matches.Any())
                throw new APArgumentException(nameof(HandlerUtils), nameof(CreateStage), nameof(matches));

            int numberOfMatches = matches.Count();
            ushort stageLevel = 0;

            while (numberOfMatches != 1)
            {
                if (numberOfMatches % 2 == 1)
                    throw new APArgumentException(nameof(HandlerUtils), nameof(CreateStage), nameof(matches));

                stageLevel++;
                numberOfMatches /= numberOfMatches;
            }

            Competition competition = await unitOfWork.CompetitionRepository.GetById(competitionId)
                ?? throw new APNotFoundException(nameof(HandlerUtils), nameof(CreateStage),
                [Tuple.Create(nameof(competitionId), competitionId)]);

            foreach (Match match in matches)
            {
                if (match.Id != competition.Id || match.Status is not MatchStatus.NOT_STARTED)
                    throw new APArgumentException(nameof(HandlerUtils), nameof(CreateStage), nameof(matches));
            }

            StageCreateDTO stage = new()
            {
                StageLevel = stageLevel,
                Matches = matches.GetIds(),
                TournamentCompetition = competition.Id,
            };

            Stage created = await unitOfWork.StageRepository.Create(stage.Adapt<Stage>());
            return created;
        }

        public static async Task<IEnumerable<Match>> CreateCompetitionMatchesUtil(IUnitOfWork unitOfWork, ulong competitionId)
        {
            async Task<IEnumerable<Match>> CreateOneVSAllCompetitionMatches(OneVSAllCompetition oneVSAllCompetition, IEnumerable<Competitor> competitors)
            {
                List<Match> matches = [];

                for (int i = 0; i < competitors.Count(); i++)
                {
                    for (int j = i + 1; j < competitors.Count(); j++)
                    {
                        Match created = await CreateMatch(unitOfWork, oneVSAllCompetition.Location, competitors.ElementAt(i).Id,
                            competitors.ElementAt(j).Id, oneVSAllCompetition.Id);
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
                    {
                        IEnumerable<Match> playedMatches = unitOfWork.MatchRepository.GetAllByCompetitorAndGameType(competitor.Id,
                            tournamentCompetition.GameType).Result;
                        double rating = GetCompetitorWinRatingOfMatchesUtil(unitOfWork, playedMatches, tournamentCompetition.Id);
                        return rating;
                    })];

                //TODO tournament competition cancel advancement logic

                List<ulong> stageMatchIds = [];
                for (int i = 0; i < competitors.Count(); i += 2)
                {
                    Match created = CreateMatch(unitOfWork, tournamentCompetition.Location, competitors.ElementAt(i).Id,
                        competitors.ElementAt(i + 1).Id, tournamentCompetition.Id).Result;
                    stageMatchIds.Add(created.Id);
                    matches.Add(created);
                }
                Stage stage = await CreateStage(unitOfWork, tournamentCompetition.Id, stageMatchIds);
                return matches;
            }

            Competition competition = await unitOfWork.CompetitionRepository.GetById(competitionId)
                ?? throw new APNotFoundException(nameof(HandlerUtils), nameof(CreateStage),
                [Tuple.Create(nameof(competitionId), competitionId)]);

            bool allMatchesWerePlayed = !(await unitOfWork.MatchRepository.GetUnfinishedByCompetition(competition.Id)).Any();

            if (!allMatchesWerePlayed)
                return [];

            IEnumerable<Competitor> firstPlaceCompetitors = await GetCompetitionFirstPlaceCompetitorsUtil(unitOfWork, competition.Id);

            if (firstPlaceCompetitors.Count() <= 1)
                return [];

            IEnumerable<Match> createdMatches = [];

            if (competition is OneVSAllCompetition oneVSAllCompetition)
                createdMatches = await CreateOneVSAllCompetitionMatches(oneVSAllCompetition, firstPlaceCompetitors);
            else if (competition is TournamentCompetition tournamentCompetition)
                createdMatches = await CreateTournamentCompetitionMatches(tournamentCompetition, firstPlaceCompetitors);

            return createdMatches;
        }
    }
}
