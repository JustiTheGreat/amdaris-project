namespace AmdarisProject.Application.Dtos.ResponseDTOs
{
    public class StageResponseDTO : ResponseDTO
    {
        public ushort? StageLevel { get; set; }
        public List<Guid> Matches { get; set; } = [];
        public Guid? TournamentCompetition { get; set; }
    }
}
