using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Domain.Models.CompetitorModels
{
    public class Competitor : Model
    {
        public required string Name { get; set; }
        public required virtual List<Match> Matches { get; set; } = [];
        public required virtual List<Match> WonMatches { get; set; } = [];
        public required virtual List<Competition> Competitions { get; set; } = [];
    }
}
