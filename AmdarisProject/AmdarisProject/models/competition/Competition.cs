using AmdarisProject.models.competitor;
using AmdarisProject.utils;
using AmdarisProject.utils.enums;
using AmdarisProject.utils.exceptions;
using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.models.competition
{
    public abstract class Competition(string name, string location, DateTime startTime, Game game) : Model
    {
        public string Name { get; set; } = name;
        public string Location { get; set; } = location;
        public DateTime StartTime { get; set; } = startTime;
        public Game Game { get; set; } = game;
        public CompetitionStatus Status { get; set; } = CompetitionStatus.ORGANIZING;
        public IEnumerable<Competitor> Competitors { get; set; } = [];
        public IEnumerable<Match> Matches { get; set; } = [];

        public bool ContainsCompetitor(Competitor competitor)
        {
            if (Game.CompetitorType is CompetitorType.PLAYER && competitor is not Player)
                throw new CompetitorException(MessageFormatter.Format(nameof(Match), nameof(ContainsCompetitor), "Competitor not matching the competition type!"));

            return Competitors.Contains(competitor) || Game.CompetitorType is CompetitorType.TWO_PLAYER_TEAM &&
                Competitors.Any(twoPlayerTeam => ((TwoPlayerTeam)twoPlayerTeam).ContainsPlayer((Player)competitor));
        }

        public void Register(Competitor competitor)
        {
            if (Status is not CompetitionStatus.ORGANIZING)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Competition), nameof(Register), Status.ToString()));

            Competitors = Competitors.Append(competitor);
            Console.WriteLine($"Competitor {competitor.Name} has registered to competition {Name}!");
        }

        public void StopRegistrations()
        {
            if (Status is not CompetitionStatus.ORGANIZING)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Competition), nameof(StopRegistrations), Status.ToString()));

            CheckCompetitorNumber();

            CreateMatches();

            Status = CompetitionStatus.NOT_STARTED;
            Console.WriteLine($"Registrations for competition {Name} have stopped!");
        }

        public void Start()
        {
            if (Status is not CompetitionStatus.NOT_STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Competition), nameof(Start), Status.ToString()));

            Status = CompetitionStatus.STARTED;
            Console.WriteLine($"Competition {Name} started!");
        }

        public void Cancel()
        {
            if (Status is not CompetitionStatus.ORGANIZING && Status is not CompetitionStatus.NOT_STARTED && Status is not CompetitionStatus.STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Competition), nameof(Cancel), Status.ToString()));

            Status = CompetitionStatus.CANCELED;
            Console.WriteLine($"Competition {Name} started!");
        }

        public void End()
        {
            bool allMatchesEnded = Matches.Where(match => match.Status is MatchStatus.FINISHED or MatchStatus.CANCELED
                or MatchStatus.SPECIAL_WIN_COMPETITOR_ONE or MatchStatus.SPECIAL_WIN_COMPETITOR_TWO).Count() == Matches.Count();
            if (!allMatchesEnded || Status is not CompetitionStatus.STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Competition), nameof(Start), Status.ToString()));

            Status = CompetitionStatus.FINISHED;
            Console.WriteLine($"Competition {Name} ended!");
        }

        protected abstract void CheckCompetitorNumber();

        protected abstract void CreateMatches();

        public abstract Competitor GetWinner();

        public int GetPoints(Competitor competitor)
        {
            if (Status is not CompetitionStatus.STARTED && Status is not CompetitionStatus.FINISHED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Competition), nameof(Start), Status.ToString()));

            return Matches.Where(match => match.ContainsCompetitor(competitor)).Select(competitor.GetPoints)
                .Aggregate((points1, points2) => points1 + points2);
        }

        public int GetPoints(Match match, Competitor competitor)
        {
            if (Status is not CompetitionStatus.STARTED && Status is not CompetitionStatus.FINISHED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Competition), nameof(GetPoints), Status.ToString()));

            if (match.Status is not MatchStatus.STARTED && match.Status is not MatchStatus.FINISHED
                && match.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_ONE && match.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Competition), nameof(GetPoints), match.Status.ToString()));

            return competitor.GetPoints(match);
        }

        public int GetWins(Competitor competitor)
        {
            return Matches
                .Where(match =>
                {
                    try
                    {
                        return match.GetWinner() is not null
                        && match.GetWinner()!.Equals(competitor);
                    }
                    catch (AmdarisProjectException)
                    {
                        return false;
                    }
                }).Count();
        }

        public IEnumerable<Competitor> GetRanking()
        {
            if (Status is not CompetitionStatus.STARTED && Status is not CompetitionStatus.FINISHED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(OneVSAllCompetition), nameof(GetWinner), Status.ToString()));

            IEnumerable<Competitor> ranking = Competitors.OrderByDescending(GetWins).ThenByDescending(GetPoints)
                .ThenBy(competitor => competitor.Name);
            return ranking;
        }
    }
}
