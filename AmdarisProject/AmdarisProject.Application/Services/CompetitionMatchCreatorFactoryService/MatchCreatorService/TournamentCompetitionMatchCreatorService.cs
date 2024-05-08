using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Services.CompetitionMatchCreatorFactoryService.MatchCreatorService
{
    public class TournamentCompetitionMatchCreatorService(IUnitOfWork unitOfWork,
        ILogger<CompetitionMatchCreatorService<TournamentCompetition>> logger)
        : CompetitionMatchCreatorService<TournamentCompetition>(unitOfWork, logger), ITournamentCompetitionMatchCreatorService
    {
        protected override bool ShouldCreateMatches(TournamentCompetition competition)
            => competition.StageLevel < Math.Log2(competition.Competitors.Count)
            && (competition.Matches.Count == 0
                || _unitOfWork.MatchRepository.AllMatchesOfCompetitonAreFinished(competition.Id).Result
                        && _unitOfWork.MatchRepository.AtLeastTwoCompetitionMatchesFromStageHaveAWinner(competition.Id, competition.StageLevel).Result);

        protected override async Task<IEnumerable<Match>> CreateMatches(TournamentCompetition competition)
        {
            IEnumerable<Match> currentStageMatches =
                await _unitOfWork.MatchRepository.GetAllByCompetitionAndStageLevel(competition.Id, competition.StageLevel);
            List<Match> createdMatches = [];

            List<Competitor> competitors;

            int newStageLevel = competition.StageLevel + 1;

            if (newStageLevel == 1)
            {
                competitors = competition.Competitors
                    .OrderByDescending(competitor =>
                        _unitOfWork.MatchRepository
                            .GetCompetitorWinRatingForGameType(competitor.Id, competition.GameFormat.GameType).Result)
                    .ToList();

                for (int i = 0; i < competitors.Count; i += 2)
                {
                    Match created = await CreateMatch(competition.Location, competitors[i].Id, competitors[i + 1].Id,
                        competition.Id, (ushort?)newStageLevel, (ushort?)(i / 2));
                    createdMatches.Add(created);
                }
            }
            else
            {
                double stageCount = Math.Log2(competition.Competitors.Count);
                double numberOfMatchesToCreate = Math.Pow(2, stageCount - newStageLevel);

                for (int stageIndex = 0; stageIndex < numberOfMatchesToCreate; stageIndex++)
                {
                    Match? matchOne = await GetMatchFromWhichToTakeTheWinnerToTakePartInThisStageLevel(
                        competition.Id, competition.StageLevel, stageIndex * 2);

                    if (matchOne is null) continue;

                    if (matchOne.Winner is null) throw new AmdarisProjectException("Match without winner!");

                    Match? matchTwo = await GetMatchFromWhichToTakeTheWinnerToTakePartInThisStageLevel(
                        competition.Id, competition.StageLevel, stageIndex * 2 + 1);

                    if (matchTwo is null) continue;

                    if (matchTwo.Winner is null) throw new AmdarisProjectException("Match without winner!");

                    Match created = await CreateMatch(competition.Location, matchOne.Winner.Id, matchTwo.Winner.Id,
                        competition.Id, (ushort?)newStageLevel, (ushort?)stageIndex);
                    createdMatches.Add(created);
                }
            }

            competition = (TournamentCompetition)(await _unitOfWork.CompetitionRepository.GetById(competition.Id)
                ?? throw new APNotFoundException(Tuple.Create(nameof(competition.Id), competition.Id)));

            competition.StageLevel = (ushort)newStageLevel;
            await _unitOfWork.CompetitionRepository.Update(competition);

            return createdMatches;
        }

        private async Task<Match?> GetMatchFromWhichToTakeTheWinnerToTakePartInThisStageLevel(Guid tournamentCompetitionId, int stageLevel, int stageIndex)
        {
            Match? match =
                await _unitOfWork.MatchRepository.GetMatchByCompetitionStageLevelAndStageIndex(
                    tournamentCompetitionId, (ushort)stageLevel, (ushort)stageIndex);

            match ??= await GetMatchFromWhichToTakeTheWinnerToTakePartInThisStageLevel(tournamentCompetitionId, stageLevel - 1, stageIndex * 2)
                ?? await GetMatchFromWhichToTakeTheWinnerToTakePartInThisStageLevel(tournamentCompetitionId, stageLevel - 1, stageIndex * 2 + 1);

            if (match is null) return match;

            if (match.Status is MatchStatus.CANCELED) return null;

            return match;
        }
    }
}
