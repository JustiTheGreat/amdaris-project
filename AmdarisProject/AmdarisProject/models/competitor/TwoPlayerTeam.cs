using AmdarisProject.utils;
using AmdarisProject.utils.enums;
using AmdarisProject.utils.exceptions;
using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.models.competitor
{
    public class TwoPlayerTeam(string name) : Competitor(name)
    {
        public Player? PlayerOne { get; set; }
        public Player? PlayerTwo { get; set; }

        public void SetPlayerOne(Player player)
        {
            if (PlayerTwo is not null && player.Equals(PlayerTwo))
                throw new SameCompetitorException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(SetPlayerOne), player.Name));

            PlayerOne = player;
        }

        public void SetPlayerTwo(Player player)
        {
            if (PlayerOne is not null && player.Equals(PlayerOne))
                throw new SameCompetitorException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(SetPlayerTwo), player.Name));

            PlayerTwo = player;
        }

        public bool ContainsPlayer(Player player)
        {
            return PlayerOne is not null && PlayerOne.Equals(player) || PlayerTwo is not null && PlayerTwo.Equals(player);
        }

        public override void InitializePointsForMatch(Match match)
        {
            if (match.Status is not MatchStatus.NOT_STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(InitializePointsForMatch), match.Status.ToString()));

            if (PlayerOne is null || PlayerTwo is null)
                throw new NullPlayerException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(InitializePointsForMatch), Name));

            PlayerOne.InitializePointsForMatch(match);
            PlayerTwo.InitializePointsForMatch(match);
        }

        public override int GetPoints(Match match)
        {
            if (match.Status is not MatchStatus.STARTED && match.Status is not MatchStatus.FINISHED
                && match.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_ONE && match.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(GetPoints), match.Status.ToString()));

            if (!match.ContainsCompetitor(this))
                throw new CompetitorException(MessageFormatter.Format(nameof(Player), nameof(GetPoints), $"Team {Name} not in match!"));

            if (PlayerOne is null || PlayerTwo is null)
                throw new NullPlayerException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(GetPoints), Name));

            return PlayerOne.GetPoints(match) + PlayerTwo.GetPoints(match);
        }

        public void AddPointsToPlayer(Player player, Match match, int points)
        {
            if (match.Status is not MatchStatus.STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Player), nameof(AddPointsToPlayer), match.Status.ToString()));

            if (!match.ContainsCompetitor(this))
                throw new CompetitorException(MessageFormatter.Format(nameof(Player), nameof(AddPointsToPlayer), $"Team {Name} not in match!"));

            if (GetPoints(match) == match.Game.WinAt)
                throw new PointsException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(AddPointsToPlayer), $"{Name} already has {GetPoints(match)} points"));

            Player scoringPlayer = (player.Equals(PlayerOne) ? PlayerOne : player.Equals(PlayerTwo) ? PlayerTwo : null)
                ?? throw new NullPlayerException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(GetPoints), $"Player {player.Name} not in team {Name}!"));

            scoringPlayer.AddPoints(match, points);

            //if (match.Game.CompetitorType == CompetitorType.TWO_PLAYER_TEAM && GetPoints(match) == match.Game.WinAt)
            //    match.End();
        }

        public override double GetRating(GameType gameType)
        {
            if (PlayerOne is null || PlayerTwo is null)
                throw new NullPlayerException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(GetRating), Name));

            double rating = (PlayerOne.GetRating(gameType) + PlayerTwo.GetRating(gameType)) / 2;
            return rating;
        }
    }
}
