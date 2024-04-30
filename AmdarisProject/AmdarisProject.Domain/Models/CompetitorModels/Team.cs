namespace AmdarisProject.Domain.Models.CompetitorModels
{
    public class Team : Competitor
    {
        public required virtual List<Player> Players { get; set; } = [];
        public required virtual List<TeamPlayer> TeamPlayers { get; set; } = [];
    }
}
