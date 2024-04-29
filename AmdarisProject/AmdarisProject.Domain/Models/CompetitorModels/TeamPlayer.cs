namespace AmdarisProject.Domain.Models.CompetitorModels
{
    public class TeamPlayer : Model
    {
        public Team Team { get; set; }
        public Player Player { get; set; }
        public bool IsActive { get; set; }
    }
}
