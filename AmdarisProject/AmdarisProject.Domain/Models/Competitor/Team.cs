using AmdarisProject.models.competition;

namespace AmdarisProject.models.competitor
{
    public class Team : Competitor
    {
        public ushort TeamSize { get; set; }
        public List<Player> Players { get; set; }

        public Team()
        {
        }

        public Team(string name, List<Match> matches, List<Competition> competitions, ushort teamSize, List<Player> players)
            : base(name, matches, competitions)
        {
            TeamSize = teamSize;
            Players = players;
        }
    }
}
