using AmdarisProject.Application.Abstractions;
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
        protected override async Task<IEnumerable<Match>> CreateMatches(TournamentCompetition tournamentCompetition)
        {
            IEnumerable<Match> currentStageMatches = tournamentCompetition.GetCurrentStageLevelMatches();
            List<Match> createdMatches = [];

            List<Competitor> competitors;

            uint newStageLevel = tournamentCompetition.StageLevel + 1;

            if (newStageLevel == 1)
            {
                competitors = tournamentCompetition.Competitors
                    .OrderByDescending(competitor =>
                        _unitOfWork.MatchRepository
                            .GetCompetitorWinRatingForGameType(competitor.Id, tournamentCompetition.GameFormat.GameType.Id).Result)
                    .ToList();

                for (int i = 0; i < competitors.Count; i += 2)
                {
                    Match created = await CreateMatch(tournamentCompetition.Location, competitors[i], competitors[i + 1],
                        tournamentCompetition, newStageLevel, (uint?)(i / 2));
                    createdMatches.Add(created);
                }
            }
            else
            {
                double stageCount = Math.Log2(tournamentCompetition.Competitors.Count);
                double numberOfMatchesToCreate = Math.Pow(2, stageCount - newStageLevel);

                for (uint stageIndex = 0; stageIndex < numberOfMatchesToCreate; stageIndex++)
                {
                    Match? matchOne = tournamentCompetition.GetMatchFromWhichToTakeTheWinnerToTakePartInThisStageLevelAndStageIndex(
                        tournamentCompetition.StageLevel, stageIndex * 2);

                    if (matchOne is null) continue;

                    if (matchOne.Winner is null) throw new AmdarisProjectException("Match without winner!");

                    Match? matchTwo = tournamentCompetition.GetMatchFromWhichToTakeTheWinnerToTakePartInThisStageLevelAndStageIndex(
                        tournamentCompetition.StageLevel, stageIndex * 2 + 1);

                    if (matchTwo is null) continue;

                    if (matchTwo.Winner is null) throw new AmdarisProjectException("Match without winner!");

                    Match created = await CreateMatch(tournamentCompetition.Location, matchOne.Winner, matchTwo.Winner,
                        tournamentCompetition, newStageLevel, stageIndex);
                    createdMatches.Add(created);
                }
            }

            tournamentCompetition.StageLevel = newStageLevel;
            await _unitOfWork.CompetitionRepository.Update(tournamentCompetition);
            return createdMatches;
        }
    }
}
