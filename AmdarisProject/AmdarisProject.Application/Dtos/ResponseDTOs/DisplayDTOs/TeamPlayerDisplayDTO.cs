namespace AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs
{
    public class TeamPlayerDisplayDTO : DisplayDTO
    {
        public required Guid TeamId { get; set; }
        public required string Team { get; set; }
        public required Guid PlayerId { get; set; }
        public required string Player { get; set; }
        public required bool IsActive { get; set; }
    }
}
