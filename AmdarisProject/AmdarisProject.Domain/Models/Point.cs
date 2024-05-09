using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Domain.Models
{
    public class Point : Model
    {
        public required uint Value { get; set; }
        public virtual required Match Match { get; set; }
        public virtual required Player Player { get; set; }
    }
}
