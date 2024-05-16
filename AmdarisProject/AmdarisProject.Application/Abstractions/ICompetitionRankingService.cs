using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Abstractions
{
    public interface ICompetitionRankingService
    {
        Task<IEnumerable<Competitor>> GetCompetitionFirstPlaceCompetitors(Guid competitionId);
        Task<IEnumerable<RankingItemDTO>> GetCompetitionRanking(Guid competitionId);
    }
}