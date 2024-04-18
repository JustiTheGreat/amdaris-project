namespace AmdarisProject.Application.Dtos.CreateDTOs.CompetitionCreateDTOs
{
    public class TournamentCompetitionCreateDTO : CompetitionCreateDTO
    {
        public IEnumerable<Guid> Stages { get; } = [];
    }
}
