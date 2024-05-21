using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
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
            .Include(o => o.GameFormat).ThenInclude(o => o.GameType)
            .Include(o => o.Competitors).ThenInclude(o => o.Matches)
            .Include(o => o.Competitors).ThenInclude(o => ((Team)o).Players)
            .Include(o => o.Matches).ThenInclude(o => o.CompetitorOne)
            .Include(o => o.Matches).ThenInclude(o => o.CompetitorTwo)
            .Include(o => o.Matches).ThenInclude(o => o.Winner)
            .FirstOrDefaultAsync(item => item.Id.Equals(id));
    }
}
