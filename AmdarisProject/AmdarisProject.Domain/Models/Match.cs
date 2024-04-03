using AmdarisProject.models.competition;
using AmdarisProject.models.competitor;
using Domain.Enums;

namespace AmdarisProject.models
{
    public class Match : Model
    {
        public string Location { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public MatchStatus Status { get; set; }
        public Competitor CompetitorOne { get; set; }
        public Competitor CompetitorTwo { get; set; }
        public Competition Competition { get; set; }
        public Stage? Stage { get; set; }
        public List<Point> Points { get; set; }

        public Match() { }

        public Match(string location, DateTime? startTime, DateTime? endTime, MatchStatus status, Competitor competitorOne,
            Competitor competitorTwo, Competition competition, Stage? stage, List<Point> points)
        {
            Location = location;
            StartTime = startTime;
            EndTime = endTime;
            Status = status;
            CompetitorOne = competitorOne;
            CompetitorTwo = competitorTwo;
            Competition = competition;
            Stage = stage;
            Points = points;
        }
    }
}
