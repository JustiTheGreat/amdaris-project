namespace AmdarisProject.Application.Dtos.ResponseDTOs
{
    public class StageResponseDTO : ResponseDTO
    {
        public ushort? StageLevel { get; set; }
        public List<ulong> Matches { get; set; } = [];
        public ulong? TournamentCompetition { get; set; }
    }
}
