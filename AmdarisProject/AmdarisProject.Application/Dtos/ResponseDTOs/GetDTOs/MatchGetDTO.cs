using AmdarisProject.Application.Dtos.DisplayDTOs;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.ResponseDTOs
{
    public class MatchGetDTO : GetDTO
    {
        public required string Location { get; set; }
        public required DateTime? StartTime { get; set; }
        public required DateTime? EndTime { get; set; }
        public required MatchStatus Status { get; set; }
        public required CompetitorDisplayDTO CompetitorOne { get; set; }
        public required CompetitorDisplayDTO CompetitorTwo { get; set; }
        public required CompetitionDisplayDTO Competition { get; set; }
        public required uint? CompetitorOnePoints { get; set; }
        public required uint? CompetitorTwoPoints { get; set; }
        public required CompetitorDisplayDTO? Winner { get; set; }
        public required ushort? StageLevel { get; set; }
        public required ushort? StageIndex { get; set; }
        public required List<PointDisplayDTO> Points { get; set; } = [];
    }
}
