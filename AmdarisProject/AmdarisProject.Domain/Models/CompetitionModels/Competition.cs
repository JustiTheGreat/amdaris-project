using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Domain.Models.CompetitionModels
{
    public abstract class Competition : Model
    {
        public required string Name { get; set; }
        public required string Location { get; set; }
        public required DateTime StartTime { get; set; }
        public required CompetitionStatus Status { get; set; }
        public required ulong? BreakInMinutes { get; set; }
        public required GameFormat GameFormat { get; set; }
        public virtual required List<Competitor> Competitors { get; set; } = [];
        public virtual required List<Match> Matches { get; set; } = [];
    }
}
