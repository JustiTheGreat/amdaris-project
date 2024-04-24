using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Domain.Extensions
{
    public static class CompetitionContainsCompetitor
    {
        public static bool ContainsCompetitor(this Competition competition, Guid competitorId)
            => competition.Competitors.Any(competitor => competitor.Id.Equals(competitorId));
    }
}
