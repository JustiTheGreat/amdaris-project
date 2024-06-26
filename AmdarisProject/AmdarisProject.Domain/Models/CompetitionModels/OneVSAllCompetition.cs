using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;

namespace AmdarisProject.Domain.Models.CompetitionModels
{
    public class OneVSAllCompetition : Competition
    {
        public override bool CantContinue()
            => Matches.All(match => match.Status is MatchStatus.CANCELED);

        public override void CheckCompetitionCompetitorNumber()
        {
            int competitorNumber = Competitors.Count;

            if (competitorNumber < 2)
                throw new APConflictException($"Competition {Name} needs at least 2 competitors (current number: {competitorNumber})!");
        }

        public override bool ShouldCreateMatches()
            => Matches.Count == 0;

        public override CompetitionStatus GetCompetitionFinishStatus()
            => Matches.Count(match =>
                match.Status == MatchStatus.FINISHED
                || match.Status == MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                || match.Status == MatchStatus.SPECIAL_WIN_COMPETITOR_TWO) > 0 ? CompetitionStatus.FINISHED : CompetitionStatus.CANCELED;
    }
}
