using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Abstractions
{
    public interface ICompetitionRankingService
    {
        Task<IEnumerable<RankingItemDTO>> GetCompetitionRanking(Guid competitionId);
        Task<IEnumerable<Competitor>> GetCompetitionWinners(Guid competitionId);
    }
}