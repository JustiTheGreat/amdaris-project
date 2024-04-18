namespace AmdarisProject.Application.Dtos.CreateDTOs.CompetitorCreateDTOs
{
    public class CompetitorCreateDTO : CreateDTO
    {
        public required string Name { get; set; }
        public List<Guid> Matches { get; set; } = [];
        public List<Guid> WonMatches { get; set; } = [];
        public List<Guid> Competitions { get; set; } = [];
    }
}
