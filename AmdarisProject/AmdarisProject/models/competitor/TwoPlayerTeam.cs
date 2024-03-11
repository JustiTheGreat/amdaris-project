using AmdarisProject.utils;

namespace AmdarisProject.models.competitor
{
    public class TwoPlayerTeam : Competitor
    {
        public Player? PlayerOne { get; set; }
        public Player? PlayerTwo { get; set; }

        public TwoPlayerTeam(string name) : base(name) { }

        public void SetPlayerOne(Player player)
        {
            if (PlayerTwo !=null && player.Equals(PlayerTwo)) 
                throw new AmdarisProjectException("Tried to add the same player to a team!");
            PlayerOne = player;
        }
        public void SetPlayerTwo(Player player)
        {
            if (PlayerOne != null && player.Equals(PlayerOne))
                throw new AmdarisProjectException("Tried to add the same player to a team!");
            PlayerTwo = player;
        }

        public override int GetPoints(Match match)
        {
            if (PlayerOne == null || PlayerTwo == null) 
                throw new AmdarisProjectException("Team does not have all the players!");
            return PlayerOne.GetPoints(match) + PlayerTwo.GetPoints(match);
        }

        public void AddPointsToPlayer(Player player, Match match, int points)
        {
            Player p = (player.Equals(PlayerOne)? PlayerOne:player.Equals(PlayerTwo)? PlayerTwo:null)
                ?? throw new AmdarisProjectException("Player not present in team!");
            if (match.Status != Status.STARTED)
                throw new AmdarisProjectException($"Can't add points to player {player}");
            p.AddPoints(match, points);
        }
    }
}
