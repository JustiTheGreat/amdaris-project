using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application
{
    public class GameRules(uint? winAt, ulong? durationInSeconds, ulong? breakInSeconds, GameType type,
        CompetitorType competitorType, ushort? teamSize)
    {
        public uint? WinAt { get; set; } = winAt;
        public ulong? DurationInSeconds { get; set; } = durationInSeconds;
        public ulong? BreakInSeconds { get; set; } = breakInSeconds;
        public GameType Type { get; set; } = type;
        public CompetitorType CompetitorType { get; set; } = competitorType;
        public ushort? TeamSize { get; set; } = teamSize;
    }
}
