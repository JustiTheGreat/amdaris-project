using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Infrastructure.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure.Persistance.Repositories
{
    public class CompetitionRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<Competition>(dbContext), ICompetitionRepository
    {
        public new async Task<Competition?> GetById(Guid id)
            => await _dbContext.Set<Competition>()
            .AsSplitQuery()
            .Include(o => o.GameFormat)
            .Include(o => o.Competitors).ThenInclude(o => o.TeamPlayers).ThenInclude(o => o.Player)
            .Include(o => o.Competitors).ThenInclude(o => o.Matches).ThenInclude(o => o.CompetitorOne)
            .Include(o => o.Competitors).ThenInclude(o => o.Matches).ThenInclude(o => o.CompetitorTwo)
            .Include(o => o.Competitors).ThenInclude(o => o.Matches).ThenInclude(o => o.Winner)
            .Include(o => o.Matches).ThenInclude(o => o.CompetitorOne)
            .Include(o => o.Matches).ThenInclude(o => o.CompetitorTwo)
            .Include(o => o.Matches).ThenInclude(o => o.Winner)
            .FirstOrDefaultAsync(item => item.Id.Equals(id));
    }
}
