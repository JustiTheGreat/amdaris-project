using AmdarisProject.utils;
using AmdarisProject.utils.enums;
using AmdarisProject.utils.exceptions;
using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.models.competitor
{
    public class Player : Competitor
    {
        public Dictionary<Match, int> Points = [];

        public Player(string name) : base(name)
        {
        }

        public override int GetPoints(Match match)
        {
            if (match.Status == MatchStatus.NOT_STARTED || match.Status == MatchStatus.CANCELED
                || match.Status == MatchStatus.SPECIAL_WIN_COMPETITOR_ONE || match.Status == MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Player), nameof(GetPoints), match.Status.ToString()));

            try
            {
                return Points[match];
            }
            catch (KeyNotFoundException)
            {
                throw new GameNotPlayedByPlayerException("");
            }
        }

        public void AddPoints(Match match, int points)
        {
            if (match.Status != MatchStatus.STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Player), nameof(AddPoints), match.Status.ToString()));

            Points[match] = Points.GetValueOrDefault(match, 0) + points;
            Console.WriteLine($"Player {Name} scored {points} points");
        }
    }
}
