using AmdarisProject.models.competitor;
using AmdarisProject.utils;
using AmdarisProject.utils.enums;

namespace AmdarisProject.models.competition
{
    public abstract class Competition(string name, string location, DateTime startTime, GameRules gameRules, CompetitionStatus status,
        List<Competitor> competitors, List<Match> matches) : Model
    {
        public string Name { get; set; } = name;
        public string Location { get; set; } = location;
        public DateTime StartTime { get; set; } = startTime;
        public GameRules GameRules { get; set; } = gameRules;
        public CompetitionStatus Status { get; set; } = status;
        public List<Competitor> Competitors { get; set; } = competitors;
        public List<Match> Matches { get; set; } = matches;
    }
}
