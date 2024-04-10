namespace AmdarisProject.Application.Dtos.CreateDTOs.CompetitionCreateDTOs
{
    public class TournamentCompetitionCreateDTO : CompetitionCreateDTO
    {
        public IEnumerable<ulong> Stages { get; } = [];
    }
}
