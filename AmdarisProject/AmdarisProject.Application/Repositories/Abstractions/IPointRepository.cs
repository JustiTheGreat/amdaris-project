using AmdarisProject.models;

namespace AmdarisProject.repositories.abstractions
{
    public interface IPointRepository : IGenericRepository<Point>
    {
        Point GetByPlayerAndMatch(ulong playerId, ulong matchId);
    }
}
