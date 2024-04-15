namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs
{
    public abstract class CompetitorResponseDTO : ResponseDTO
    {
        public string? Name { get; set; }
        public List<Guid> Matches { get; set; } = [];
        public List<Guid> Competitions { get; set; } = [];
    }
}
