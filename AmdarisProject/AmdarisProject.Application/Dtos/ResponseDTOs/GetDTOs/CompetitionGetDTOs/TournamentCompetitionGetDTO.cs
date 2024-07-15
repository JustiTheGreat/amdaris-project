namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs
{
    public class TournamentCompetitionGetDTO : CompetitionGetDTO
    {
        public required uint StageLevel { get; set; }
    }
}
