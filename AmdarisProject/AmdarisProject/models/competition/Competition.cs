using AmdarisProject.models.competitor;
using AmdarisProject.utils;
using AmdarisProject.utils.Exceptions;

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
        public CompetitionStatus Status { get; set; }
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
            Status = CompetitionStatus.ORGANIZING;
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
            if (!Status.Equals(CompetitionStatus.ORGANIZING))
                throw new IllegalStatusException($"{nameof(StopRegistrations)}: {Status}");
            Status = CompetitionStatus.NOT_STARTED;
            Console.WriteLine($"Registrations for competition {Name} have stopped!");
        }

        public void Start()
        {
            if (!Status.Equals(CompetitionStatus.NOT_STARTED))
                throw new IllegalStatusException($"{nameof(Start)}: {Status}");
            Status = CompetitionStatus.STARTED;
            Console.WriteLine($"Competition {Name} started!");
        }

        public int GetPoints(Competitor competitor)
        {
            return Matches.Where(match => match.CompetitorOne.Equals(competitor) && match.CompetitorOne.Equals(competitor)
            || match.CompetitorTwo.Equals(competitor) && match.CompetitorTwo.Equals(competitor))
                .Select(match => match.CompetitorOne.Equals(competitor) ? match.GetPointsCompetitorOne() : match.GetPointsCompetitorTwo())
                .Aggregate((points1, points2) => points1 + points2);
        }

        public int GetPoints(Match match, Competitor competitor)
        {
            if (Status.Equals(CompetitionStatus.ORGANIZING) || Status.Equals(MatchStatus.NOT_STARTED)
                || Status.Equals(MatchStatus.CANCELED))
                throw new IllegalStatusException($"{nameof(GetPoints)}: {Status}");
            if (match.Status.Equals(MatchStatus.NOT_STARTED) || match.Status.Equals(MatchStatus.CANCELED)
                || match.Status.Equals(MatchStatus.SPECIAL_WIN_COMPETITOR_ONE) || match.Status.Equals(MatchStatus.SPECIAL_WIN_COMPETITOR_TWO))
                throw new IllegalStatusException($"{nameof(GetPoints)}: {match.Status}");
            return competitor.GetPoints(match);
        }

        protected Competition(int id, string name, string location, DateTime startTime, GameType gameType, CompetitorType competitorType,
            CompetitionStatus status, IEnumerable<Competitor> competitors, IEnumerable<Match> matches)
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
