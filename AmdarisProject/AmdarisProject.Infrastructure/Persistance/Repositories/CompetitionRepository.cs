using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Application.Common.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Infrastructure.Persistance.Contexts;
using AmdarisProject.Infrastructure.Persistance.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure.Persistance.Repositories
{
    public class CompetitionRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<Competition>(dbContext), ICompetitionRepository
    {
        public new async Task<Competition?> GetById(Guid id)
        {
            Competition? competition = await _dbContext.Set<Competition>()
               .AsSplitQuery()
               .Include(o => o.GameFormat).ThenInclude(o => o.GameType)
               .Include(o => o.Competitors).ThenInclude(o => o.Matches)
               .Include(o => o.Competitors).ThenInclude(o => ((Team)o).Players)
               .Include(o => o.Competitors).ThenInclude(o => ((Team)o).TeamPlayers)
               .Include(o => o.Matches).ThenInclude(o => o.CompetitorOne)
               .Include(o => o.Matches).ThenInclude(o => o.CompetitorTwo)
               .Include(o => o.Matches).ThenInclude(o => o.Winner)
               .FirstOrDefaultAsync(item => item.Id.Equals(id));
            if (competition is not null)
                competition.Matches = competition.Matches.OrderBy(match => match.ActualizedStartTime ?? DateTimeOffset.MinValue)
               .ToList();
            return competition;
        }

        public new async Task<Tuple<IEnumerable<Competition>, int>> GetPaginatedData(PagedRequest pagedRequest)
            => await _dbContext.Set<Competition>()
            .Include(o => o.GameFormat).ThenInclude(o => o.GameType)
            .CreatePaginatedResultAsync(pagedRequest);
    }
}
