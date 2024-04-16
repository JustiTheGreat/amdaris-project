using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure.Repositories
{
    public class PointRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<Point>(dbContext), IPointRepository
    {
        public async Task<Point?> GetByPlayerAndMatch(Guid playerId, Guid matchId)
            => await _dbContext.Set<Point>().FirstOrDefaultAsync(point => point.Player.Id.Equals(playerId) && point.Match.Id.Equals(matchId));

        public async Task<IEnumerable<Point>> GetByPlayersAndMatch(IEnumerable<Guid> playerIds, Guid matchId)
        {
            IEnumerable<Point> points = await _dbContext.Set<Point>()
                .Where(point => playerIds.Contains(point.Player.Id) && point.Match.Id.Equals(matchId)).ToListAsync();
            bool allIdsWereFound = playerIds.All(id => points.Any(point => point.Id.Equals(id)));

            if (!allIdsWereFound) throw new APNotFoundException(nameof(playerIds));

            return points;
        }
    }
}
