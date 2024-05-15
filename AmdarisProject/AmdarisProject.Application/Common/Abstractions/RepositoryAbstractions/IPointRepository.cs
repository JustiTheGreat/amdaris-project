using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Common.Abstractions.RepositoryAbstractions
{
    public interface IPointRepository : IGenericRepository<Point>
    {
        Task<Point?> GetByPlayerAndMatch(Guid playerId, Guid matchId);
    }
}
