using AmdarisProject.utils.enums;

namespace AmdarisProject.utils
{
    public class GameRules(uint? winAt, ulong? durationInSeconds, ulong? breakInSeconds, GameType type, ushort teamSize = 1)
    {
        public uint? WinAt { get; set; } = winAt;
        public ulong? DurationInSeconds { get; set; } = durationInSeconds;
        public ulong? BreakInSeconds { get; set; } = breakInSeconds;
        public GameType Type { get; set; } = type;
        public CompetitorType CompetitorType { get; set; } = teamSize == 1 ? CompetitorType.PLAYER : CompetitorType.TEAM;
        public ushort TeamSize { get; set; } = teamSize;
    }
}
