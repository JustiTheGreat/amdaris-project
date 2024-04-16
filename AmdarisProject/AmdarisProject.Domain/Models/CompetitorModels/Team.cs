using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Domain.Models.CompetitorModels
{
    public class Team : Competitor
    {
        public ushort TeamSize { get; set; }
        public virtual List<Player> Players { get; set; } = [];

        public Team() : base()
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
