using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;

namespace AmdarisProject.Application.Dtos.ResponseDTOs
{
    public class PointGetDTO : GetDTO
    {
        public required uint Value { get; set; }
        public required Guid Match { get; set; }
        public required CompetitorDisplayDTO Player { get; set; }
    }
}
