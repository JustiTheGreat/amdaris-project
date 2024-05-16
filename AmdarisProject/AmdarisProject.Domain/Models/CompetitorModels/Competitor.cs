using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Domain.Models.CompetitorModels
{
    public abstract class Competitor : Model
    {
        public required string Name { get; set; }
        public virtual required List<Match> Matches { get; set; } = [];
        public virtual required List<Match> WonMatches { get; set; } = [];
        public virtual required List<Competition> Competitions { get; set; } = [];

        public abstract bool IsOrContainsCompetitor(Guid competitorId);
    }
}
