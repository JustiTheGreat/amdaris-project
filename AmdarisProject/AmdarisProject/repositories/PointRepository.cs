using AmdarisProject.models;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.repositories
{
    public class PointRepository : GenericRepository<Point>, IPointRepository
    {
        public override Point Update(Point point)
        {
            if (point is null)
                throw new APArgumentException(nameof(PointRepository), nameof(Update), nameof(point));

            Point stored = GetById(point.Id);
            stored.Value = point.Value;
            return stored;
        }

        public Point GetByPlayerAndMatch(ulong playerId, ulong matchId)
        {
            return _dataSet.FirstOrDefault(point => point.Player.Id == playerId && point.Match.Id == matchId)
                ?? throw new APNotFoundException(nameof(PointRepository), nameof(GetByPlayerAndMatch), nameof(Point));
        }
    }
}
