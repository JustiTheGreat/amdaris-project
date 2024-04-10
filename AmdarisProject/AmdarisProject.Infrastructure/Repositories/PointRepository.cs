using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure.Repositories
{
    public class PointRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<Point>(dbContext), IPointRepository
    {
        public async Task<Point?> GetByPlayerAndMatch(ulong playerId, ulong matchId)
            => await _dbContext.Set<Point>().FirstOrDefaultAsync(point => point.Player.Id == playerId && point.Match.Id == matchId);
    }
}
