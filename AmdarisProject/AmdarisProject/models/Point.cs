using AmdarisProject.models.competitor;

namespace AmdarisProject.models
{
    public class Point(ushort value, Match match, Player player) : Model
    {
        public uint Value { get; set; } = value;
        public Match Match { get; set; } = match;
        public Player Player { get; set; } = player;
    }
}
