using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.ExtensionMethods
{
    public static class CompetitorIsOrIsPartOfCompetitor
    {
        public static bool IsOrContainsCompetitor(this Competitor competitor, Guid competitorId)
            => competitor.Id.Equals(competitorId)
            || competitor is Team team && team.ContainsPlayer(competitorId);
    }
}
