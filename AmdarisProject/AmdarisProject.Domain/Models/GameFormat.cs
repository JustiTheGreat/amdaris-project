using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Domain.Models
{
    public class GameFormat : Model
    {
        public required string Name { get; set; }
        public required GameType GameType { get; set; }
        public required CompetitorType CompetitorType { get; set; }
        public required ushort? TeamSize { get; set; }
        public required uint? WinAt { get; set; }
        public required ulong? DurationInMinutes { get; set; }
    }
}
