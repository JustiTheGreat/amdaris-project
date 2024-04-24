using AmdarisProject.Application.Dtos.DisplayDTOs;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.ResponseDTOs
{
    public class MatchResponseDTO : ResponseDTO
    {
        public string Location { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public MatchStatus Status { get; set; }
        public CompetitorDisplayDTO CompetitorOne { get; set; }
        public CompetitorDisplayDTO CompetitorTwo { get; set; }
        public CompetitionDisplayDTO Competition { get; set; }
        public uint? CompetitorOnePoints { get; set; }
        public uint? CompetitorTwoPoints { get; set; }
        public CompetitorDisplayDTO? Winner { get; set; }
        public ushort? StageLevel { get; set; }
        public ushort? StageIndex { get; set; }
        public List<PointDisplayDTO> Points { get; set; } = [];
    }
}
