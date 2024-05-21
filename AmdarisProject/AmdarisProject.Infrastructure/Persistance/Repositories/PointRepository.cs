using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Infrastructure.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure.Persistance.Repositories
{
    public class PointRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<Point>(dbContext), IPointRepository
    {
        public async Task<Point?> GetByMatchAndPlayer(Guid matchId, Guid playerId)
            => await _dbContext.Set<Point>()
            .AsSplitQuery()
            .Include(o => o.Match).ThenInclude(o => o.CompetitorOne).ThenInclude(o => ((Team)o).Players)
            .Include(o => o.Match).ThenInclude(o => o.CompetitorTwo).ThenInclude(o => ((Team)o).Players)
            .Include(o => o.Match).ThenInclude(o => o.Competition).ThenInclude(o => o.GameFormat)
            .Include(o => o.Player)
            .FirstOrDefaultAsync(point => point.Match.Id.Equals(matchId) && point.Player.Id.Equals(playerId));
    }
}
