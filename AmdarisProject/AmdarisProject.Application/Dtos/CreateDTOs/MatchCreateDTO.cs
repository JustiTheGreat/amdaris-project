using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.CreateDTOs
{
    public class MatchCreateDTO : CreateDTO
    {
        public required string Location { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public required MatchStatus Status { get; set; }
        public required ulong CompetitorOne { get; set; }
        public required ulong CompetitorTwo { get; set; }
        public required ulong Competition { get; set; }
        public ulong? Stage { get; set; }
        public List<ulong> Points { get; set; } = [];
    }
}
