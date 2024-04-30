namespace AmdarisProject.Domain.Models.CompetitorModels
{
    public class TeamPlayer : Model
    {
        public required Team Team { get; set; }
        public required Player Player { get; set; }
        public required bool IsActive { get; set; }
    }
}
