using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Models.CompetitionModels;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure.Repositories
{
    public class CompetitionRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<Competition>(dbContext), ICompetitionRepository
    {
        public new async Task<Competition?> GetById(Guid id)
            => await _dbContext.Set<Competition>()
            //.AsSplitQuery()
            //.Include(o => o.GameFormat)
            //.Include(o => o.Competitors)
            //.Include(o => o.Matches)
            .FirstOrDefaultAsync(item => item.Id.Equals(id));
    }
}
