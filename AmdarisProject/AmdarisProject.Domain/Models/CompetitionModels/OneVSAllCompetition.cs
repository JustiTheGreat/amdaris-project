using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Domain.Models.CompetitionModels
{
    public class OneVSAllCompetition : Competition
    {
        public override bool CantContinue()
            => Matches.All(match => match.Status is MatchStatus.CANCELED);
    }
}
