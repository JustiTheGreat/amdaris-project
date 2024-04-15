using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure.Repositories
{
    public class PointRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<Point>(dbContext), IPointRepository
    {
        public async Task<Point?> GetByPlayerAndMatch(Guid playerId, Guid matchId)
            => await _dbContext.Set<Point>().FirstOrDefaultAsync(point => point.Player.Id.Equals(playerId) && point.Match.Id.Equals(matchId));
    }
}
