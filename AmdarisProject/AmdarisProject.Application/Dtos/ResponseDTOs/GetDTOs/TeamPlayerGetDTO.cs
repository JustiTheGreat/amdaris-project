namespace AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs
{
    public class TeamPlayerGetDTO : GetDTO
    {
        public required Guid TeamId { get; set; }
        public required Guid PlayerId { get; set; }
        public required bool IsActive { get; set; }
    }
}
