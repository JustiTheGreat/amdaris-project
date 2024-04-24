using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;

namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs
{
    public class TeamResponseDTO : CompetitorResponseDTO
    {
        public ushort TeamSize { get; set; }
        public List<PlayerDisplayDTO> Players { get; set; } = [];
    }
}
