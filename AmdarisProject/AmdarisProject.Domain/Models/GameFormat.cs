using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Domain.Models
{
    public class GameFormat : Model
    {
        public required string Name { get; set; }
        public required GameType GameType { get; set; }
        public required CompetitorType CompetitorType { get; set; }
        public required uint? TeamSize { get; set; }
        public required uint? WinAt { get; set; }
        public required ulong? DurationInMinutes { get; set; }

        public bool HasValidWinningConditions()
            => WinAt is not null && WinAt > 0
            || DurationInMinutes is not null && DurationInMinutes > 0;

        public bool HasValidCompetitorRequirements()
            => CompetitorType is CompetitorType.PLAYER && TeamSize is null
            || CompetitorType is CompetitorType.TEAM && TeamSize is not null && TeamSize >= 2;
    }
}
