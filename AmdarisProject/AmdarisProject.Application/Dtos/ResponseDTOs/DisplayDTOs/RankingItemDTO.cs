namespace AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs
{
    public class RankingItemDTO : DisplayDTO
    {
        public required string Competitor { get; set; }
        public required int Wins { get; set; }
        public required int Points { get; set; }
        public required string? ProfilePicture { get; set; }
    }
}
