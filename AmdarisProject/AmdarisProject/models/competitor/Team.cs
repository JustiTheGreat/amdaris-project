using AmdarisProject.utils;
using AmdarisProject.utils.enums;
using AmdarisProject.utils.exceptions;
using AmdarisProject.utils.Exceptions;
using System.Text.RegularExpressions;

namespace AmdarisProject.models.competitor
{
    public class Team(string name, int teamSize) : Competitor(name)
    {
        public int TeamSize { get; set; } = teamSize;
        public List<Player> Players { get; set; } = [];

        public void AddPlayer(Player player)
        {
            if (player is null)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(Team), nameof(AddPlayer), nameof(player)));

            if (Players.Count == TeamSize)
                throw new CompetitorNumberException(MessageFormatter.Format(nameof(Team), nameof(AddPlayer), $"Team {Name} is full!"));

            if (Players.Contains(player))
                throw new CompetitorException(MessageFormatter.Format(nameof(Team), nameof(AddPlayer),
                    $"Player {player.Name} is already a member of team {Name}!"));

            Players.Add(player);
        }

        public bool ContainsPlayer(Player player) => player is not null && Players.Contains(player);

        public override void InitializePointsForMatch(Match match)
        {
            if (match is null)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(Team), nameof(InitializePointsForMatch), nameof(match)));

            if (match.Status is not MatchStatus.NOT_STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Team), nameof(InitializePointsForMatch), match.Status.ToString()));

            if (Players.Count < TeamSize)
                throw new CompetitorNumberException(MessageFormatter.Format(nameof(Team), nameof(InitializePointsForMatch),
                    $"Team {Name} is needs {TeamSize - Players.Count} more members!"));

            if (Players.Any(player => player is null))
                throw new NullReferenceException(MessageFormatter.Format(nameof(Team), nameof(InitializePointsForMatch), Name));

            Players.ForEach(player => player.InitializePointsForMatch(match));
        }

        public override int GetPoints(Match match)
        {
            if (match is null)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(Team), nameof(GetPoints), nameof(match)));

            if (match.Status is not MatchStatus.STARTED && match.Status is not MatchStatus.FINISHED
                && match.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_ONE && match.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Team), nameof(GetPoints), match.Status.ToString()));

            if (!match.ContainsCompetitor(this))
                throw new CompetitorException(MessageFormatter.Format(nameof(Team), nameof(GetPoints),
                    $"Team {Name} not in match!"));

            if (Players.Count < TeamSize)
                throw new CompetitorNumberException(MessageFormatter.Format(nameof(Team), nameof(GetPoints),
                    $"Team {Name} is needs {TeamSize - Players.Count} more members!"));

            if (Players.Any(player => player is null))
                throw new NullReferenceException(MessageFormatter.Format(nameof(Team), nameof(GetPoints), Name));

            return Players.Aggregate(0, (total, player) => total + player.GetPoints(match));
        }

        public void AddPointsToPlayer(Player player, Match match, int points)
        {
            if (player is null)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(Team), nameof(AddPointsToPlayer), nameof(player)));

            if (match is null)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(Team), nameof(AddPointsToPlayer), nameof(match)));

            if (points <= 0)
                throw new PointsException(MessageFormatter.Format(nameof(Team), nameof(AddPointsToPlayer), "Bad points value!"));

            if (match.Status is not MatchStatus.STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Team), nameof(AddPointsToPlayer), match.Status.ToString()));

            if (!match.ContainsCompetitor(this))
                throw new CompetitorException(MessageFormatter.Format(nameof(Team), nameof(AddPointsToPlayer),
                    $"Team {Name} not in match!"));

            if (GetPoints(match) == match.Game.WinAt)
                throw new PointsException(MessageFormatter.Format(nameof(Team), nameof(AddPointsToPlayer),
                    $"{Name} already has {GetPoints(match)} points!"));

            if (Players.Count < TeamSize)
                throw new CompetitorNumberException(MessageFormatter.Format(nameof(Team), nameof(AddPointsToPlayer),
                    $"Team {Name} is needs {TeamSize - Players.Count} more members!"));

            if (Players.Any(player => player is null))
                throw new NullReferenceException(MessageFormatter.Format(nameof(Team), nameof(AddPointsToPlayer), Name));

            if (!ContainsPlayer(player))
                throw new NullReferenceException(MessageFormatter.Format(nameof(Team), nameof(AddPointsToPlayer),
                    $"Player {player.Name} not in team {Name}!"));

            player.AddPoints(match, points);
        }

        public override double GetRating(GameType gameType)
        {
            if (Players.Count < TeamSize)
                throw new CompetitorNumberException(MessageFormatter.Format(nameof(Team), nameof(GetRating),
                    $"Team {Name} is needs {TeamSize - Players.Count} more members!"));

            if (Players.Any(player => player is null))
                throw new NullReferenceException(MessageFormatter.Format(nameof(Team), nameof(GetRating), Name));

            return Players.Aggregate(0.0, (total, player) => total + player.GetRating(gameType)) / TeamSize;
        }
    }
}
