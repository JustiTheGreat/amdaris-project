namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs
{
    public class TeamResponseDTO : CompetitorResponseDTO
    {
        public ushort TeamSize { get; set; }
        public List<Guid> Players { get; set; } = [];
    }
}
