namespace AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs
{
    public class CompetitionDisplayDTO : DisplayDTO
    {
        public required string Name { get; set; }
        public required string CompetitionType { get; set; }
        public required string Status { get; set; }
        public required string GameType { get; set; }
        public required string CompetitorType { get; set; }
    }
}
