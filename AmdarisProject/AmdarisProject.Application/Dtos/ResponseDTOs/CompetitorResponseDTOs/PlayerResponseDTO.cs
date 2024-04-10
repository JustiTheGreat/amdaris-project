namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs
{
    public class PlayerResponseDTO : CompetitorResponseDTO
    {
        public List<ulong> Points { get; set; } = [];
        public List<ulong> Teams { get; set; } = [];
    }
}
