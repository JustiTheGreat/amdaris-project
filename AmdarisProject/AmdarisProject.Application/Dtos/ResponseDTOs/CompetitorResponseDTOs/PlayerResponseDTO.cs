using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;

namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs
{
    public class PlayerResponseDTO : CompetitorResponseDTO
    {
        public List<Guid> Points { get; set; } = [];
        public List<TeamDisplayDTO> Teams { get; set; } = [];
    }
}
