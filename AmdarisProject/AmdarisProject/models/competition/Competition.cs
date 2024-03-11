using AmdarisProject.models.competitor;
using AmdarisProject.utils;

namespace AmdarisProject.models.competition
{
    public class Competition : ICloneable
    {
        private static int instances = 0;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public GameType GameType { get; set; }
        public CompetitorType CompetitorType { get; set; }
        public Status Status { get; set; }
        public IEnumerable<Competitor> Competitors { get; set; }
        public IEnumerable<Match> Matches { get; set; }

        protected Competition(string name, string location, DateTime startTime, GameType gameType, CompetitorType competitorType)
        {
            Id = ++instances;
            Name = name;
            Location = location;
            StartTime = startTime;
            GameType = gameType;
            CompetitorType = competitorType;
            Status = Status.ORGANIZING;
            Competitors = [];
            Matches = [];
        }

        public void Register(Competitor competitor)
        {
            Competitors = Competitors.Append(competitor);
            Console.WriteLine($"Competitor {competitor.Name} have stopped!");
        }

        public void StopRegistrations()
        {
            if (!Status.Equals(Status.ORGANIZING))
                throw new AmdarisProjectException("Wrong status for stopping the registrations!");
            Status = Status.NOT_STARTED;
            Console.WriteLine($"Registrations for competition {Name} have stopped!");
        }

        public void Start()
        {
            if (!Status.Equals(Status.NOT_STARTED))
                throw new AmdarisProjectException("Wrong status for starting the competition!");
            Status = Status.STARTED;
            Console.WriteLine($"Competition {Name} started!");
        }

        public int GetPoints(Competitor competitor)
        {
            return Matches.Where(match => match.CompetitorOne.Equals(competitor) || match.CompetitorTwo.Equals(competitor))
                .Select(match => match.GetPoints(competitor))
                .Aggregate((points1, points2) => points1 + points2);
        }

        public int GetPoints(Match match, Competitor competitor)
        {
            return competitor.GetPoints(match);
        }

        protected Competition(int id, string name, string location, DateTime startTime, GameType gameType, CompetitorType competitorType,
            Status status, IEnumerable<Competitor> competitors, IEnumerable<Match> matches)
        {
            Id = id;
            Name = name;
            Location = location;
            StartTime = startTime;
            GameType = gameType;
            Status = status;
            CompetitorType = competitorType;
            Competitors = competitors;
            Matches = matches;
        }

        public virtual object Clone()
        {
            return new Competition(
                Id,
                Name,
                Location,
                StartTime,
                GameType,
                CompetitorType,
                Status,
                Competitors,
                Matches
                );
        }
    }
}
