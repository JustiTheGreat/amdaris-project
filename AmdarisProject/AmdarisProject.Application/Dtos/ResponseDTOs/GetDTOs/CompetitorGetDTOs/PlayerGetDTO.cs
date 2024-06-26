using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;

namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs
{
    public class PlayerGetDTO : CompetitorGetDTO
    {
        public required List<Guid> Points { get; set; } = [];
        public required List<CompetitorDisplayDTO> Teams { get; set; } = [];
        public required string? ProfilePicture { get; set; }
    }
}
