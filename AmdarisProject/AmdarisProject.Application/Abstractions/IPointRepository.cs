using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Abstractions
{
    public interface IPointRepository : IGenericRepository<Point>
    {
        Task<Point?> GetByPlayerAndMatch(ulong playerId, ulong matchId);
    }
}
