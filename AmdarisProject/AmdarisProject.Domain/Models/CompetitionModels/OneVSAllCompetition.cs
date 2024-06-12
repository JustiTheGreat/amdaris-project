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
    }
}
