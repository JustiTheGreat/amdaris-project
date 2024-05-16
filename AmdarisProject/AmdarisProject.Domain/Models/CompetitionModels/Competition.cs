using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Domain.Models.CompetitionModels
{
    public abstract class Competition : Model
    {
        public required string Name { get; set; }
        public required string Location { get; set; }
        public required DateTime StartTime { get; set; }
        public required CompetitionStatus Status { get; set; }
        public required ulong? BreakInMinutes { get; set; }
        public virtual required GameFormat GameFormat { get; set; }
        public virtual required List<Competitor> Competitors { get; set; } = [];
        public virtual required List<Match> Matches { get; set; } = [];

        public bool AllMatchesOfCompetitonAreDone()
            => Matches.All(match => match.Status == MatchStatus.FINISHED
                || match.Status == MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                || match.Status == MatchStatus.SPECIAL_WIN_COMPETITOR_TWO
                || match.Status == MatchStatus.CANCELED);

        public abstract bool CantContinue();

        public bool ContainsCompetitor(Guid competitorId)
            => Competitors.Any(competitor => competitor.Id.Equals(competitorId));

        public int GetCompetitorPoints(Guid competitorId)
            => Matches
            .Where(match => match.CompetitorOnePoints is not null
                && match.CompetitorTwoPoints is not null
                && (match.CompetitorOne.Id.Equals(competitorId) || match.CompetitorTwo.Id.Equals(competitorId)))
            .Select(match => match.CompetitorOne.Id.Equals(competitorId)
                ? (int)match.CompetitorOnePoints! : (int)match.CompetitorTwoPoints!)
            .Aggregate(0, (result, point) => result + point);

        public int GetCompetitorWins(Guid competitorId)
            => Matches.Where(match => match.Winner != null && match.Winner.Id.Equals(competitorId)).Count();
    }
}
