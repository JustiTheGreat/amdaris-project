using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Abstractions
{
    public interface IPointRepository : IGenericRepository<Point>
    {
        Task<Point?> GetByPlayerAndMatch(Guid playerId, Guid matchId);

        Task<IEnumerable<Point>> GetByPlayersAndMatch(IEnumerable<Guid> playerIds, Guid matchId);
    }
}
