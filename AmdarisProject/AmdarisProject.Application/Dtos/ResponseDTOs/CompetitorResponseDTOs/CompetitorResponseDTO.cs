namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs
{
    public abstract class CompetitorResponseDTO : ResponseDTO
    {
        public string? Name { get; set; }
        public List<ulong> Matches { get; set; } = [];
        public List<ulong> Competitions { get; set; } = [];
    }
}
