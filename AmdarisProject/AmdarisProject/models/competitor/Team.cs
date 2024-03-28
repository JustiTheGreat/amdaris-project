using AmdarisProject.models.competition;
using AmdarisProject.utils.exceptions;
using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.models.competitor
{
    public class Team(string name, List<Match> matches, List<Competition> competitions, ushort teamSize, List<Player> players)
        : Competitor(name, matches, competitions)
    {
        public ushort TeamSize { get; set; } = teamSize;
        public List<Player> Players { get; set; } = players;

        public void AddPlayer(Player player)
        {
            if (player is null)
                throw new APArgumentException(nameof(Team), nameof(AddPlayer), nameof(player));

            if (Players.Count == TeamSize)
                throw new APCompetitorNumberException(nameof(Team), nameof(AddPlayer), $"Team {Name} is full!");

            if (Players.Contains(player))
                throw new APCompetitorException(nameof(Team), nameof(AddPlayer), $"Player {player.Name} is already a member of team {Name}!");

            Players.Add(player);
        }
    }
}
