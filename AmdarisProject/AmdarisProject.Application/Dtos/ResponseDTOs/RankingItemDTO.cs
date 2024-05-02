namespace AmdarisProject.Application.Dtos.ResponseDTOs
{
    public class RankingItemDTO(Guid competitorId, string competitorName, uint wins, uint points)
    {
        public Guid CompetitorId { get; set; } = competitorId;
        public string CompetitorName { get; set; } = competitorName;
        public uint Wins { get; set; } = wins;
        public uint Points { get; set; } = points;
    }
}
