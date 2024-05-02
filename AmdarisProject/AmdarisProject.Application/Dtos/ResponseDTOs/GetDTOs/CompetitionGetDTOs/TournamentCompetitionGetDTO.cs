namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs
{
    public class TournamentCompetitionGetDTO : CompetitionGetDTO
    {
        public required ushort StageLevel { get; set; }
    }
}
