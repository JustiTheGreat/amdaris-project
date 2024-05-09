using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Domain.Extensions
{
    public static class GetCompetitionMatchFromWhichToTakeWinnerToTakePartInStageLevelAndStageIndex
    {
        public static Match? GetMatchFromWhichToTakeTheWinnerToTakePartInThisStageLevelAndStageIndex(
            this TournamentCompetition tournamentCompetition, uint stageLevel, uint stageIndex)
        {
            Match? match = tournamentCompetition.Matches
                .Where(match => match.StageLevel == stageLevel && match.StageIndex == stageIndex)
                .FirstOrDefault();

            match ??= tournamentCompetition.GetMatchFromWhichToTakeTheWinnerToTakePartInThisStageLevelAndStageIndex(
                    stageLevel - 1, stageIndex * 2)
                ?? tournamentCompetition.GetMatchFromWhichToTakeTheWinnerToTakePartInThisStageLevelAndStageIndex(
                    stageLevel - 1, stageIndex * 2 + 1);

            if (match is null) return match;

            if (match.Status is MatchStatus.CANCELED) return null;

            return match;
        }
    }
}
