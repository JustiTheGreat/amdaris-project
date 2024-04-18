using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Application.ExtensionMethods
{
    public static class CompetitionContainsCompetitor
    {
        public static bool ContainsCompetitor(this Competition competition, Guid competitorId)
            => competition.Competitors.Any(competitor => competitor.IsOrContainsCompetitor(competitorId));
    }
}
