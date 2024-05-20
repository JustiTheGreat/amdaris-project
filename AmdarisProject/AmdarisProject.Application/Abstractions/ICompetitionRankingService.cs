using AmdarisProject.Application.Dtos.ResponseDTOs;

namespace AmdarisProject.Application.Abstractions
{
    public interface ICompetitionRankingService
    {
        Task<IEnumerable<RankingItemDTO>> GetCompetitionRanking(Guid competitionId);
    }
}