namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs
{
    public class TournamentCompetitionResponseDTO : CompetitionResponseDTO
    {
        public IEnumerable<ulong> Stages { get; set; } = [];
    }
}
