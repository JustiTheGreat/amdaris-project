using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.CreateDTOs.CompetitionCreateDTOs
{
    public class CompetitionCreateDTO : CreateDTO
    {
        public required string Name { get; set; }
        public required string Location { get; set; }
        public required DateTime StartTime { get; set; }
        public required CompetitionStatus Status { get; set; }
        public ulong? BreakInSeconds { get; set; }
        public Guid GameFormat { get; set; }
        public List<Guid> Competitors { get; set; } = [];
        public List<Guid> Matches { get; set; } = [];
    }
}
