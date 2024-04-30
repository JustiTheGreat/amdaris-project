using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Domain.Models.CompetitionModels
{
    public class Competition : Model
    {
        public required string Name { get; set; }
        public required string Location { get; set; }
        public required DateTime StartTime { get; set; }
        public required CompetitionStatus Status { get; set; }
        public required ulong? BreakInSeconds { get; set; }
        public required GameFormat GameFormat { get; set; }
        public required virtual List<Competitor> Competitors { get; set; } = [];
        public required virtual List<Match> Matches { get; set; } = [];
    }
}
