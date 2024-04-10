using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.CreateDTOs.CompetitionCreateDTOs
{
    public class CompetitionCreateDTO : CreateDTO
    {
        public required string Name { get; init; }
        public required string Location { get; init; }
        public required DateTime StartTime { get; init; }
        public required CompetitionStatus Status { get; init; }
        public uint? WinAt { get; set; }
        public ulong? DurationInSeconds { get; set; }
        public ulong? BreakInSeconds { get; set; }
        public required GameType GameType { get; init; }
        public required CompetitorType CompetitorType { get; init; }
        public ushort? TeamSize { get; set; }
        public List<ulong> Competitors { get; set; } = [];
        public List<ulong> Matches { get; set; } = [];
    }
}
