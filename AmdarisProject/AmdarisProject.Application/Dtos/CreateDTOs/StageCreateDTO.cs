namespace AmdarisProject.Application.Dtos.CreateDTOs
{
    public class StageCreateDTO : CreateDTO
    {
        public required ushort StageLevel { get; set; }
        public List<ulong> Matches { get; set; } = [];
        public required ulong TournamentCompetition { get; set; }
    }
}
