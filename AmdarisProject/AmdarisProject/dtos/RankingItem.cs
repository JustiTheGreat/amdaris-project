namespace AmdarisProject.dtos
{
    public class RankingItem
    {
        public ulong CompetitorId { get; set; }
        public string CompetitorName { get; set; }
        public uint Wins { get; set; }
        public uint Points { get; set; }
    }
}
