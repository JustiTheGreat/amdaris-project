using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;

namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs
{
    public class TeamGetDTO : CompetitorGetDTO
    {
        public required List<CompetitorDisplayDTO> Players { get; set; } = [];
    }
}
