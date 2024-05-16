using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Domain.Models.CompetitionModels
{
    public class TournamentCompetition : Competition
    {
        public required uint StageLevel { get; set; }

        public override bool CantContinue()
            => StageLevel > 0
            && Matches.Where(match => match.StageLevel == StageLevel).Any()
            && Matches
                .Where(match => match.StageLevel == StageLevel)
                .All(match => match.Status is MatchStatus.CANCELED);

        public bool AtLeastTwoMatchesFromTheCurrentStageHaveAWinner()
            => Matches.Count(match => match.StageLevel == StageLevel
                && (match.Status == MatchStatus.FINISHED
                    || match.Status == MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                    || match.Status == MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)) >= 2;

        public IEnumerable<Match> GetCurrentStageLevelMatches()
            => Matches
            .Where(match => match.StageLevel == StageLevel)
            .OrderBy(match => match.StageIndex)
            .ToList();

        public Match? GetMatchFromWhichToTakeTheWinnerToTakePartInThisStageLevelAndStageIndex(uint stageLevel, uint stageIndex)
        {
            Match? match = Matches
                .Where(match => match.StageLevel == stageLevel && match.StageIndex == stageIndex)
                .FirstOrDefault();

            match ??= GetMatchFromWhichToTakeTheWinnerToTakePartInThisStageLevelAndStageIndex(
                    stageLevel - 1, stageIndex * 2)
                ?? GetMatchFromWhichToTakeTheWinnerToTakePartInThisStageLevelAndStageIndex(
                    stageLevel - 1, stageIndex * 2 + 1);

            if (match is null) return match;

            if (match.Status is MatchStatus.CANCELED) return null;

            return match;
        }
    }
}
