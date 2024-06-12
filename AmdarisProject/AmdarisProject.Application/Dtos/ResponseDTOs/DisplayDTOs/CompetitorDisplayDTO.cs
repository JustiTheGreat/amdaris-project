namespace AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs
{
    public class CompetitorDisplayDTO : DisplayDTO
    {
        public required string Name { get; set; }
        public required string CompetitorType { get; set; }
        public required int NumberOfCompetitions { get; set; }
        public required int NumberOfMatches { get; set; }
        public required int? NumberOfTeams { get; set; }
        public required int? NumberOfPlayers { get; set; }
        public required int? NumberOfActivePlayers { get; set; }
    }
}
