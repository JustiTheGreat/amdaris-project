namespace AmdarisProject.Domain.Models.CompetitionModels
{
    public class TournamentCompetition : Competition
    {
        public required uint StageLevel { get; set; }
    }
}
