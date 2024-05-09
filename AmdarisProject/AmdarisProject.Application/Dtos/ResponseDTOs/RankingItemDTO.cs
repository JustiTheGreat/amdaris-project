namespace AmdarisProject.Application.Dtos.ResponseDTOs
{
    public class RankingItemDTO(Guid competitorId, string competitorName, int wins, int points)
    {
        public Guid CompetitorId { get; set; } = competitorId;
        public string CompetitorName { get; set; } = competitorName;
        public int Wins { get; set; } = wins;
        public int Points { get; set; } = points;
    }
}
