using AmdarisProject.utils;
using AmdarisProject.utils.enums;
using AmdarisProject.utils.exceptions;
using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.models.competitor
{
    public class Player(string name) : Competitor(name)
    {
        public readonly Dictionary<Match, int> Points = [];

        public override void InitializePointsForMatch(Match match)
        {
            if (match is null)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(Player), nameof(InitializePointsForMatch), nameof(match)));

            if (match.Status is not MatchStatus.NOT_STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Player), nameof(InitializePointsForMatch), match.Status.ToString()));

            Points[match] = 0;
        }

        public override int GetPoints(Match match)
        {
            if (match is null)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(Player), nameof(GetPoints), nameof(match)));

            if (match.Status is not MatchStatus.STARTED
                && match.Status is not MatchStatus.FINISHED
                && match.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                && match.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Player), nameof(GetPoints), match.Status.ToString()));

            if (!match.ContainsCompetitor(this))
                throw new CompetitorException(MessageFormatter.Format(nameof(Player), nameof(GetPoints),
                    $"Player {Name} not in match!"));

            try
            {
                return Points[match];
            }
            catch (KeyNotFoundException)
            {
                throw new CompetitorException(MessageFormatter.Format(nameof(Player), nameof(GetPoints),
                    $"Match not played by player {Name}!"));
            }
        }

        public void AddPoints(Match match, int points)
        {
            if (match is null)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(Player), nameof(AddPoints), nameof(match)));

            if (points <= 0)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(Player), nameof(AddPoints), "Bad points value!"));

            if (match.Status is not MatchStatus.STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Player), nameof(AddPoints), match.Status.ToString()));

            if (!match.ContainsCompetitor(this))
                throw new CompetitorException(MessageFormatter.Format(nameof(Player), nameof(AddPoints), $"Player {Name} not in match!"));

            if (match.GetPointsCompetitorOne() == match.Game.WinAt
                || match.GetPointsCompetitorOne() == match.Game.WinAt)
                throw new PointsException(MessageFormatter.Format(nameof(Player), nameof(AddPoints), $"{Name} already has {Points[match]} points!"));

            try
            {
                Points[match] = Points[match] + points;
            }
            catch (KeyNotFoundException)
            {
                throw new PointsException(MessageFormatter.Format(nameof(Player), nameof(AddPoints),
                    $"Player {Name} doesn't have points for match!"));
            }
            Console.WriteLine($"Player {Name} scored {points} points");
        }

        public override double GetRating(GameType gameType)
        {
            IEnumerable<Match> matchesOfGameTypePlayed = Points
                .Where(point => point.Key.Game.Type == gameType)
                .Select(point => point.Key).ToList();

            if (!matchesOfGameTypePlayed.Any())
                return 0;

            int matchesOfGameTypeWon = matchesOfGameTypePlayed
                .Where(match => match.Game.CompetitorType is CompetitorType.PLAYER ?
                    (match.GetWinner()?.Equals(this) ?? false)
                    : (((match.GetWinner()) as Team)?.ContainsPlayer(this) ?? false))
                .Count();

            return (double)matchesOfGameTypeWon / matchesOfGameTypePlayed.Count();
        }
    }
}
