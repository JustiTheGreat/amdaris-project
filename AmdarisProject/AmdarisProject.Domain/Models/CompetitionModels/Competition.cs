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
        public uint? WinAt { get; set; }
        public ulong? DurationInSeconds { get; set; }
        public ulong? BreakInSeconds { get; set; }
        public GameType GameType { get; set; }
        public CompetitorType CompetitorType { get; set; }
        public ushort? TeamSize { get; set; }
        public virtual List<Competitor> Competitors { get; set; } = [];
        public virtual List<Match> Matches { get; set; } = [];

        public Competition()
        {
        }

        protected Competition(string name, string location, DateTime startTime, CompetitionStatus status,
            uint? winAt, ulong? durationInSeconds, ulong? breakInSeconds, GameType gameType, CompetitorType competitorType, ushort? teamSize,
            List<Competitor> competitors, List<Match> matches)
        {
            Name = name;
            Location = location;
            StartTime = startTime;
            Status = status;
            WinAt = winAt;
            DurationInSeconds = durationInSeconds;
            BreakInSeconds = breakInSeconds;
            GameType = gameType;
            CompetitorType = competitorType;
            TeamSize = teamSize;
            Competitors = competitors;
            Matches = matches;
        }
    }
}
