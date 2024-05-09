using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Domain.Extensions
{
    public static class GetCompetitionCompetitorWins
    {
        public static int GetCompetitorWins(this Competition competition, Guid competitorId)
            => competition.Matches.Where(match => match.Winner != null && match.Winner.Id.Equals(competitorId)).Count();
    }
}
