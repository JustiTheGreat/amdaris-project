using AmdarisProject.Application.Dtos;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Services
{
    public interface ICompetitionRankingService
    {
        Task<IEnumerable<Competitor>> GetCompetitionFirstPlaceCompetitors(Guid competitionId);
        Task<IEnumerable<RankingItemDTO>> GetCompetitionRanking(Guid competitionId);
    }
}