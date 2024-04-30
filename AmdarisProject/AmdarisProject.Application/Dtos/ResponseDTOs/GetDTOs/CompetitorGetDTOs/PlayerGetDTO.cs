using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;

namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs
{
    public class PlayerGetDTO : CompetitorGetDTO
    {
        public required List<Guid> Points { get; set; } = [];
        public required List<TeamDisplayDTO> Teams { get; set; } = [];
    }
}
