using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Domain.Extensions
{
    public static class GetCompetitionCompetitorPoints
    {
        public static int GetCompetitorPoints(this Competition competition, Guid competitorId)
            => competition.Matches
            .Where(match => match.CompetitorOnePoints is not null
                && match.CompetitorTwoPoints is not null
                && (match.CompetitorOne.Id.Equals(competitorId) || match.CompetitorTwo.Id.Equals(competitorId)))
            .Select(match => match.CompetitorOne.Id.Equals(competitorId)
                ? (int)match.CompetitorOnePoints! : (int)match.CompetitorTwoPoints!)
            .Aggregate(0, (result, point) => result + point);
    }
}
