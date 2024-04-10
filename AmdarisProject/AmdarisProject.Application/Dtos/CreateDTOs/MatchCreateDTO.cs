using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.CreateDTOs
{
    public class MatchCreateDTO : CreateDTO
    {
        public required string Location { get; init; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public required MatchStatus Status { get; init; }
        public required ulong CompetitorOne { get; init; }
        public required ulong CompetitorTwo { get; init; }
        public required ulong Competition { get; init; }
        public ulong? Stage { get; set; }
        public List<ulong> Points { get; set; } = [];
    }
}
