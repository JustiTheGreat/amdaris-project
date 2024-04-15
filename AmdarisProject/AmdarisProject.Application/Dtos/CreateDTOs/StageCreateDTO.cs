namespace AmdarisProject.Application.Dtos.CreateDTOs
{
    public class StageCreateDTO : CreateDTO
    {
        public required ushort StageLevel { get; set; }
        public List<Guid> Matches { get; set; } = [];
        public required Guid TournamentCompetition { get; set; }
    }
}
