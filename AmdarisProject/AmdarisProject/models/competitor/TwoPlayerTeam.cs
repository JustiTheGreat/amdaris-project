using AmdarisProject.utils;
using AmdarisProject.utils.enums;
using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.models.competitor
{
    public class TwoPlayerTeam : Competitor
    {
        public Player? PlayerOne { get; set; }
        public Player? PlayerTwo { get; set; }

        public TwoPlayerTeam(string name) : base(name) { }

        public void SetPlayerOne(Player player)
        {
            if (PlayerTwo != null && player.Equals(PlayerTwo))
                throw new SameCompetitorException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(SetPlayerOne), player.Name));

            PlayerOne = player;
        }

        public void SetPlayerTwo(Player player)
        {
            if (PlayerOne != null && player.Equals(PlayerOne))
                throw new SameCompetitorException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(SetPlayerTwo), player.Name));

            PlayerTwo = player;
        }

        public override int GetPoints(Match match)
        {
            if (match.Status == MatchStatus.NOT_STARTED || match.Status == MatchStatus.CANCELED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(GetPoints), match.Status.ToString()));

            if (PlayerOne == null || PlayerTwo == null)
                throw new NullPlayerException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(GetPoints), Name));

            return PlayerOne.GetPoints(match) + PlayerTwo.GetPoints(match);
        }

        public void AddPointsToPlayer(Player player, Match match, int points)
        {
            Player p = (player.Equals(PlayerOne) ? PlayerOne : player.Equals(PlayerTwo) ? PlayerTwo : null)
                ?? throw new SameCompetitorException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(GetPoints), "Player not present in team!"));

            if (match.Status != MatchStatus.STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(GetPoints), match.Status.ToString()));

            p.AddPoints(match, points);
        }
    }
}
