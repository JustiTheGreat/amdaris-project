using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;

namespace AmdarisProject.Application.Dtos.ResponseDTOs
{
    public class RankingItemDTO()
    {
        public required CompetitorDisplayDTO Competitor { get; set; }
        public required int Wins { get; set; }
        public required int Points { get; set; }
    }
}
