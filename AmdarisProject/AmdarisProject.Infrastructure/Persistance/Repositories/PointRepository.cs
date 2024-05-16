using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure.Persistance.Repositories
{
    public class PointRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<Point>(dbContext), IPointRepository
    {
        public async Task<Point?> GetByPlayerAndMatch(Guid playerId, Guid matchId)
            => await _dbContext.Set<Point>()
            .AsSplitQuery()
            .Include(o => o.Match)
            .Include(o => o.Player)
            .FirstOrDefaultAsync(point => point.Player.Id.Equals(playerId) && point.Match.Id.Equals(matchId));
    }
}
