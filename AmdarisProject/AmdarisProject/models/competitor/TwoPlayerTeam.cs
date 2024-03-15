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
            if (player is null)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(SetPlayerOne), nameof(player)));

            if (player.Equals(PlayerTwo))
                throw new CompetitorException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(SetPlayerOne),
                    $"Player {player.Name} is already a member of team {Name}!"));

            PlayerOne = player;
        }

        public void SetPlayerTwo(Player player)
        {
            if (player is null)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(SetPlayerTwo), nameof(player)));

            if (player.Equals(PlayerOne))
                throw new CompetitorException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(SetPlayerTwo),
                    $"Player {player.Name} is already a member of team {Name}!"));

            PlayerTwo = player;
        }

        public bool ContainsPlayer(Player? player)
            => player is not null
                && ((PlayerOne?.Equals(player) ?? false)
                    || (PlayerTwo?.Equals(player) ?? false));

        public override void InitializePointsForMatch(Match match)
        {
            if (match is null)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(InitializePointsForMatch), nameof(match)));

            if (match.Status is not MatchStatus.NOT_STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(InitializePointsForMatch), match.Status.ToString()));

            if (PlayerOne is null || PlayerTwo is null)
                throw new NullReferenceException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(InitializePointsForMatch), Name));

            PlayerOne.InitializePointsForMatch(match);
            PlayerTwo.InitializePointsForMatch(match);
        }

        public override int GetPoints(Match match)
        {
            if (match is null)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(GetPoints), nameof(match)));

            if (match.Status is not MatchStatus.STARTED && match.Status is not MatchStatus.FINISHED
                && match.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_ONE && match.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(GetPoints), match.Status.ToString()));

            if (!match.ContainsCompetitor(this))
                throw new CompetitorException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(GetPoints),
                    $"Team {Name} not in match!"));

            if (PlayerOne is null || PlayerTwo is null)
                throw new NullReferenceException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(GetPoints), Name));

            return PlayerOne.GetPoints(match) + PlayerTwo.GetPoints(match);
        }

        public void AddPointsToPlayer(Player player, Match match, int points)
        {
            if (player is null)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(AddPointsToPlayer), nameof(player)));

            if (match is null)
                throw new ArgumentNullException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(AddPointsToPlayer), nameof(match)));

            if (points <= 0)
                throw new PointsException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(AddPointsToPlayer), "Bad points value!"));

            if (match.Status is not MatchStatus.STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(AddPointsToPlayer), match.Status.ToString()));

            if (!match.ContainsCompetitor(this))
                throw new CompetitorException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(AddPointsToPlayer),
                    $"Team {Name} not in match!"));

            if (GetPoints(match) == match.Game.WinAt)
                throw new PointsException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(AddPointsToPlayer),
                    $"{Name} already has {GetPoints(match)} points!"));

            Player scoringPlayer = (player.Equals(PlayerOne) ? PlayerOne : player.Equals(PlayerTwo) ? PlayerTwo : null)
                ?? throw new NullReferenceException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(GetPoints),
                $"Player {player.Name} not in team {Name}!"));

            scoringPlayer.AddPoints(match, points);
        }

        public override double GetRating(GameType gameType)
        {
            if (PlayerOne is null || PlayerTwo is null)
                throw new NullReferenceException(MessageFormatter.Format(nameof(TwoPlayerTeam), nameof(GetRating), Name));

            return (PlayerOne.GetRating(gameType) + PlayerTwo.GetRating(gameType)) / 2;
        }
    }
}
