using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.ExtensionMethods
{
    public static class MatchContainsCompetitior
    {
        public static bool ContainsCompetitor(this Match match, Guid competitorId)
            => match.CompetitorOne.IsOrContainsCompetitor(competitorId)
            || match.CompetitorTwo.IsOrContainsCompetitor(competitorId);
    }
}
