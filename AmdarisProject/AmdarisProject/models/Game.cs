using AmdarisProject.utils.enums;

namespace AmdarisProject.models
{
    public class Game(int winAt, GameType type, CompetitorType competitorType)
    {
        public int WinAt { get; set; } = winAt;
        public GameType Type { get; set; } = type;
        public CompetitorType CompetitorType { get; set; } = competitorType;
    }
}
