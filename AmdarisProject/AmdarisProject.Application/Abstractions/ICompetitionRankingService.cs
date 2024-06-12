using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;

namespace AmdarisProject.Application.Abstractions
{
    public interface ICompetitionRankingService
    {
        Task<IEnumerable<RankingItemDTO>> GetCompetitionRanking(Guid competitionId);
    }
}