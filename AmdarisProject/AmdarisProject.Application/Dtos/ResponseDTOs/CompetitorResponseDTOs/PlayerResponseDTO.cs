namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs
{
    public class PlayerResponseDTO : CompetitorResponseDTO
    {
        public List<Guid> Points { get; set; } = [];
        public List<Guid> Teams { get; set; } = [];
    }
}
