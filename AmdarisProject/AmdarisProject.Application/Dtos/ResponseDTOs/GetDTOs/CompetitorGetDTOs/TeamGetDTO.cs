using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;

namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs
{
    public class TeamGetDTO : CompetitorGetDTO
    {
        public required List<PlayerDisplayDTO> Players { get; set; } = [];
    }
}
