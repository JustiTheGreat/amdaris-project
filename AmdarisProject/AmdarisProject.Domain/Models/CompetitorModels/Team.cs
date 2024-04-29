namespace AmdarisProject.Domain.Models.CompetitorModels
{
    public class Team : Competitor
    {
        public virtual List<Player> Players { get; set; } = [];
        public virtual List<TeamPlayer> TeamPlayers { get; set; } = [];
    }
}
