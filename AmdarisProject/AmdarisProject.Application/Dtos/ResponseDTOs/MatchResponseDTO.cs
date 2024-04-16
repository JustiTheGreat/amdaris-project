using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.ResponseDTOs
{
    public class MatchResponseDTO : ResponseDTO
    {
        public string Location { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public MatchStatus Status { get; set; }
        public Guid CompetitorOne { get; set; }
        public Guid CompetitorTwo { get; set; }
        public Guid Competition { get; set; }
        public uint? CompetitorOnePoints { get; set; }
        public uint? CompetitorTwoPoints { get; set; }
        public Guid? Winner { get; set; }
        public ushort? StageLevel { get; set; }
        public ushort? StageIndex { get; set; }
        public List<Guid> Points { get; set; } = [];
    }
}
