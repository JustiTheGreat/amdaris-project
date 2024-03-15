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
                throw new CompetitorException(MessageFormatter.Format(nameof(Competition), nameof(ContainsCompetitor),
                    "Competitor not matching the competition type!"));

            return competitor is not null
                && (Competitors.Contains(competitor)
                    || Competitors.Any(c => (c as TwoPlayerTeam)?.ContainsPlayer(competitor as Player) ?? false));
        }

        public void Register(Competitor competitor)
        {
            if (competitor is null)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(Competition), nameof(Register), nameof(competitor)));

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

            CreateMatches(Competitors);

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
            if (Status is not CompetitionStatus.ORGANIZING
                && Status is not CompetitionStatus.NOT_STARTED
                && Status is not CompetitionStatus.STARTED)
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

        protected abstract void CreateMatches(IEnumerable<Competitor> competitors);

        public abstract Competitor GetWinner();

        public int GetPoints(Competitor competitor)
        {
            if (competitor is null)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(Competition), nameof(GetPoints), nameof(competitor)));

            if (Status is not CompetitionStatus.STARTED
                && Status is not CompetitionStatus.FINISHED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Competition), nameof(Start), Status.ToString()));

            return Matches.Where(match => match.ContainsCompetitor(competitor))
                .Select(competitor.GetPoints)
                .Aggregate((points1, points2) => points1 + points2);
        }

        public int GetPoints(Match match, Competitor competitor)
        {
            if (match is null)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(Competition), nameof(GetPoints), nameof(match)));

            if (competitor is null)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(Competition), nameof(GetPoints), nameof(competitor)));

            if (Status is not CompetitionStatus.STARTED
                && Status is not CompetitionStatus.FINISHED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Competition), nameof(GetPoints), Status.ToString()));

            if (match.Status is not MatchStatus.STARTED
                && match.Status is not MatchStatus.FINISHED
                && match.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                && match.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Competition), nameof(GetPoints), match.Status.ToString()));

            return competitor.GetPoints(match);
        }

        public int GetWins(Competitor competitor)
            => Matches.Where(match => match.GetWinner()?.Equals(competitor) ?? false).Count();

        public IEnumerable<KeyValuePair<Competitor, int>> GetRanking()
        {
            if (Status is not CompetitionStatus.STARTED
                && Status is not CompetitionStatus.FINISHED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(OneVSAllCompetition), nameof(GetWinner), Status.ToString()));

            IEnumerable<KeyValuePair<Competitor, int>> ranking = Competitors
                .Select(competitor => new KeyValuePair<Competitor, int>(competitor, GetWins(competitor)))
                .OrderByDescending(entry => entry.Value)
                .ThenByDescending(entry => GetPoints(entry.Key))
                .ThenBy(entry => entry.Key.Name);

            return ranking;
        }

        public IEnumerable<Match> GetUnfinishedMatches()
            => Matches.Where(match => match.Status is MatchStatus.NOT_STARTED or MatchStatus.STARTED).ToList();


        public void CreateBonusMatches()
        {
            IEnumerable<KeyValuePair<Competitor, int>> ranking = GetRanking();
            int maxWinsOnCompetition = ranking.First().Value;
            IEnumerable<Competitor> winners = ranking
                .Where(entry => entry.Value == maxWinsOnCompetition)
                .Select(entry => entry.Key).ToList();

            if (winners.Count() > 1)
                CreateMatches(winners);
        }
    }
}
