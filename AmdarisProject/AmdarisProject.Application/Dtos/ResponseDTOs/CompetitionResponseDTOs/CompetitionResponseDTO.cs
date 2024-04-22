using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs
{
    public class CompetitionResponseDTO : ResponseDTO
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public CompetitionStatus Status { get; set; }
        public ulong? BreakInSeconds { get; set; }
        public GameType GameType { get; set; }
        public CompetitorType CompetitorType { get; set; }
        public ushort? TeamSize { get; set; }
        public uint? WinAt { get; set; }
        public ulong? DurationInSeconds { get; set; }
        public List<Guid> Competitors { get; set; } = [];
        public List<Guid> Matches { get; set; } = [];
    }
}
