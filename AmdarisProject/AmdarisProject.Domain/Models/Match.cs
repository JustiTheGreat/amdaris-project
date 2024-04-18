using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Domain.Models
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
        public uint? CompetitorOnePoints { get; set; }
        public uint? CompetitorTwoPoints { get; set; }
        public Competitor? Winner { get; set; }
        public ushort? StageLevel { get; set; }
        public ushort? StageIndex { get; set; }
        public virtual List<Point> Points { get; set; } = [];

        public Match() { }

        public Match(string location, DateTime? startTime, DateTime? endTime, MatchStatus status, Competitor competitorOne,
            Competitor competitorTwo, Competition competition, uint? competitorOnePoints, uint? competitorTwoPoints,
            Competitor? winner, ushort? stageLevel, ushort? stageIndex, List<Point> points)
        {
            Location = location;
            StartTime = startTime;
            EndTime = endTime;
            Status = status;
            CompetitorOne = competitorOne;
            CompetitorTwo = competitorTwo;
            Competition = competition;
            CompetitorOnePoints = competitorOnePoints;
            CompetitorTwoPoints = competitorTwoPoints;
            Winner = winner;
            StageLevel = stageLevel;
            StageIndex = stageIndex;
            Points = points;
        }
    }
}
