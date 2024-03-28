using AmdarisProject.models.competition;
using AmdarisProject.models.competitor;
using AmdarisProject.utils.enums;

namespace AmdarisProject.models
{
    public class Match(string location, DateTime? startTime, DateTime? endTime, MatchStatus matchStatus, Competitor competitorOne,
        Competitor competitorTwo, Competition competition, Stage? stage, List<Point> points) : Model
    {
        public string Location { get; set; } = location;
        public DateTime? StartTime { get; set; } = startTime;
        public DateTime? EndTime { get; set; } = endTime;
        public MatchStatus Status { get; set; } = matchStatus;
        public Competitor CompetitorOne { get; set; } = competitorOne;
        public Competitor CompetitorTwo { get; set; } = competitorTwo;
        public Competition Competition { get; set; } = competition;
        public Stage? Stage { get; set; } = stage;
        public List<Point> Points { get; set; } = points;
    }
}
