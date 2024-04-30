using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Domain.Models
{
    public class Point : Model
    {
        public required uint Value { get; set; }
        public required Match Match { get; set; }
        public required Player Player { get; set; }
    }
}
