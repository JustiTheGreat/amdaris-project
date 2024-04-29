namespace AmdarisProject.Domain.Models.CompetitorModels
{
    public class Player : Competitor
    {
        public virtual List<Point> Points { get; set; } = [];
        public virtual List<Team> Teams { get; set; } = [];
        public virtual List<TeamPlayer> TeamPlayers { get; set; } = [];
    }
}
