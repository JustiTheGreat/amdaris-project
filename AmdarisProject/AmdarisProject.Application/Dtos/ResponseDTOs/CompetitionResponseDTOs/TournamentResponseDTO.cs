namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs
{
    public class TournamentCompetitionResponseDTO : CompetitionResponseDTO
    {
        public IEnumerable<Guid> Stages { get; set; } = [];
    }
}
