using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;

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

        public override void CheckCompetitionCompetitorNumber()
        {
            int competitorNumber = Competitors.Count;

            if (competitorNumber < 2)
                throw new APConflictException($"Competition {Name} needs a power of 2 number of competitors bigger than 1 (current number: {competitorNumber})!");

            while (competitorNumber != 1)
            {
                if (competitorNumber % 2 == 1)
                    throw new APConflictException($"Competition {Name} needs a power of 2 number of competitors bigger than 1 (current number: {competitorNumber})!");

                competitorNumber /= 2;
            }
        }

        public override bool ShouldCreateMatches()
            => StageLevel < Math.Log2(Competitors.Count)
            && (Matches.Count == 0 || AllMatchesOfCompetitonAreDone() && AtLeastTwoMatchesFromTheCurrentStageHaveAWinner());

        public override CompetitionStatus GetCompetitionFinishStatus()
            => Matches.Count(match => match.StageLevel == StageLevel
                && (match.Status == MatchStatus.FINISHED
                    || match.Status == MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                    || match.Status == MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)) == 1 ? CompetitionStatus.FINISHED : CompetitionStatus.CANCELED;

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
