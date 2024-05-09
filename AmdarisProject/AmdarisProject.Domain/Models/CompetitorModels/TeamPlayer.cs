namespace AmdarisProject.Domain.Models.CompetitorModels
{
    public class TeamPlayer : Model
    {
        public virtual required Team Team { get; set; }
        public virtual required Player Player { get; set; }
        public virtual required bool IsActive { get; set; }
    }
}
