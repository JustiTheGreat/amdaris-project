using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Domain.Models
{
    public class Point : Model
    {
        public uint Value { get; set; }
        public Match Match { get; set; }
        public Player Player { get; set; }

        public Point()
        {
        }

        public Point(uint value, Match match, Player player)
        {
            Value = value;
            Match = match;
            Player = player;
        }
    }
}
