namespace AmdarisProject.Domain.Models.CompetitorModels
{
    public class Player : Competitor
    {
        public virtual required List<Point> Points { get; set; } = [];
        public virtual required List<Team> Teams { get; set; } = [];

        public override bool IsOrContainsCompetitor(Guid competitorId)
            => Id.Equals(competitorId);

        public bool NotInCompetition(Guid competitionId)
            => Competitions.All(competition => competition.Id.Equals(competitionId) && !competition.ContainsPlayer(Id));
    }
}
