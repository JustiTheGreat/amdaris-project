using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.CreateDTOs.CompetitionCreateDTOs
{
    public class CompetitionCreateDTO : CreateDTO
    {
        public required string Name { get; set; }
        public required string Location { get; set; }
        public required DateTime StartTime { get; set; }
        public required CompetitionStatus Status { get; set; }
        public uint? WinAt { get; set; }
        public ulong? DurationInSeconds { get; set; }
        public ulong? BreakInSeconds { get; set; }
        public required GameType GameType { get; set; }
        public required CompetitorType CompetitorType { get; set; }
        public ushort? TeamSize { get; set; }
        public List<Guid> Competitors { get; set; } = [];
        public List<Guid> Matches { get; set; } = [];
    }
}
