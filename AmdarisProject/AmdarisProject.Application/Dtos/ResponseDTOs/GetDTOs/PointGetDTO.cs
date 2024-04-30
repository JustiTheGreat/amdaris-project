using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;

namespace AmdarisProject.Application.Dtos.ResponseDTOs
{
    public class PointGetDTO : GetDTO
    {
        public required uint Value { get; set; }
        public required Guid Match { get; set; }
        public required PlayerDisplayDTO Player { get; set; }
    }
}
