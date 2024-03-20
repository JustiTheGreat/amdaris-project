using AmdarisProject.utils.enums;

namespace AmdarisProject.utils
{
    public class GameRules(int? winAt, long? durationInSeconds, long breakInSeconds, GameType type, int teamSize = 1)
    {
        public int? WinAt { get; set; } = winAt;
        public long? DurationInSeconds { get; set; } = durationInSeconds;
        public long BreakInSeconds { get; set; } = breakInSeconds;
        public GameType Type { get; set; } = type;
        public CompetitorType CompetitorType { get; set; } = teamSize == 1 ? CompetitorType.PLAYER : CompetitorType.TEAM;
        public int TeamSize { get; set; } = teamSize;
    }
}
