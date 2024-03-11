using AmdarisProject.models.competitor;
using AmdarisProject.utils;

namespace AmdarisProject.models
{
    public class Match
    {
        private static int instances = 0;
        public int Id { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public Competitor CompetitorOne { get; set; }
        public Competitor CompetitorTwo { get; set; }
        public GameType GameType { get; set; }
        public Status Status { get; set; }

        public Match(string location, DateTime startTime, GameType gameType, Competitor competitorOne, Competitor competitorTwo)
        {
            if (competitorOne.Equals(competitorTwo))
                throw new AmdarisProjectException("Tried to create a match with the same competitors for both sides!");
            Id = ++instances;
            Location = location;
            StartTime = startTime;
            CompetitorOne = competitorOne;
            CompetitorTwo = competitorTwo;
            GameType = gameType;
            Status = Status.NOT_STARTED;
        }

        public void Start()
        {
            Status = Status.STARTED;
        }

        public int GetPoints(Competitor competitor)
        {
            if (Status.Equals(Status.NOT_STARTED))
                throw new AmdarisProjectException("Tried to get the points for a match that hasn't started yet!");
            return competitor.GetPoints(this);
        }
    }
}
