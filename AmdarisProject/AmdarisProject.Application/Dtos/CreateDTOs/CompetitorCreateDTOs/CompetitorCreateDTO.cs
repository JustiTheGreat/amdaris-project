namespace AmdarisProject.Application.Dtos.CreateDTOs.CompetitorCreateDTOs
{
    public class CompetitorCreateDTO : CreateDTO
    {
        public required string Name { get; init; }
        public List<ulong> Matches { get; set; } = [];
        public List<ulong> Competitions { get; set; } = [];
    }
}
