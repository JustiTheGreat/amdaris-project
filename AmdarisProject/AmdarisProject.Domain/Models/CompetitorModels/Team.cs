namespace AmdarisProject.Domain.Models.CompetitorModels
{
    public class Team : Competitor
    {
        public virtual required List<Player> Players { get; set; } = [];
        public virtual required List<TeamPlayer> TeamPlayers { get; set; } = [];
    }
}
