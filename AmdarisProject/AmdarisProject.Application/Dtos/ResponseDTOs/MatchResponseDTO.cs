using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.ResponseDTOs
{
    public class MatchResponseDTO : ResponseDTO
    {
        public string? Location { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public MatchStatus? Status { get; set; }
        public ulong? CompetitorOne { get; set; }
        public ulong? CompetitorTwo { get; set; }
        public ulong? Competition { get; set; }
        public ulong? Stage { get; set; }
        public List<ulong> Points { get; set; } = [];
    }
}
