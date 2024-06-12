using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Domain.Models
{
    public class TeamPlayer : Model
    {
        public virtual required Team Team { get; set; }
        public virtual required Player Player { get; set; }
        public virtual required bool IsActive { get; set; }

        public void ChangeStatus()
        {
            if (Team.IsInAStartedMatch())
                throw new APException($"Team {Team.Name} is in a started match!");

            IsActive = !IsActive;
        }
    }
}
