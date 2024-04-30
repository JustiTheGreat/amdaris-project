namespace AmdarisProject.Domain.Models.CompetitorModels
{
    public class Player : Competitor
    {
        public required virtual List<Point> Points { get; set; } = [];
        public required virtual List<Team> Teams { get; set; } = [];
        public required virtual List<TeamPlayer> TeamPlayers { get; set; } = [];
    }
}
