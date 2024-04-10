namespace AmdarisProject.Application.Dtos
{
    public class RankingItemDTO(ulong competitorId, string competitorName, uint wins, uint points)
    {
        public ulong CompetitorId { get; set; } = competitorId;
        public string CompetitorName { get; set; } = competitorName;
        public uint Wins { get; set; } = wins;
        public uint Points { get; set; } = points;
    }
}
