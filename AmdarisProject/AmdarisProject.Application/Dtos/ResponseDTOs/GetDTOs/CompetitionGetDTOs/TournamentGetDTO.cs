namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs
{
    public class TournamentCompetitionResponseDTO : CompetitionGetDTO
    {
        public required ushort StageLevel { get; set; }
    }
}
