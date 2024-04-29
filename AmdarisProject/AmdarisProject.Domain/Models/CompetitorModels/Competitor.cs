using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Domain.Models.CompetitorModels
{
    public class Competitor : Model
    {
        public string Name { get; set; }
        public virtual List<Match> Matches { get; set; } = [];
        public virtual List<Match> WonMatches { get; set; } = [];
        public virtual List<Competition> Competitions { get; set; } = [];
    }
}
