using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.DisplayDTOs
{
    public class CompetitionDisplayDTO : DisplayDTO
    {
        public required string Name { get; set; }
        public required CompetitionStatus Status { get; set; }
        public required GameType GameType { get; set; }
        public required CompetitorType CompetitorType { get; set; }
    }
}
