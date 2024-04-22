using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Domain.Models.CompetitionModels
{
    public class Competition : Model
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public CompetitionStatus Status { get; set; }
        public ulong? BreakInSeconds { get; set; }
        public GameFormat GameFormat { get; set; }
        public virtual List<Competitor> Competitors { get; set; } = [];
        public virtual List<Match> Matches { get; set; } = [];

        public Competition()
        {
        }

        protected Competition(string name, string location, DateTime startTime, CompetitionStatus status,
            ulong? breakInSeconds, GameFormat gameFormat, List<Competitor> competitors, List<Match> matches)
        {
            Name = name;
            Location = location;
            StartTime = startTime;
            Status = status;
            BreakInSeconds = breakInSeconds;
            GameFormat = gameFormat;
            Competitors = competitors;
            Matches = matches;
        }
    }
}
