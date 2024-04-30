namespace AmdarisProject.Domain.Models.CompetitionModels
{
    public class TournamentCompetition : Competition
    {
        public required ushort StageLevel { get; set; }
    }
}
