using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.DisplayDTOs
{
    public class CompetitionDisplayDTO
    {
        public string Name { get; set; }
        public CompetitionStatus Status { get; set; }
        public GameType GameType { get; set; }
        public CompetitorType CompetitorType { get; set; }
    }
}
