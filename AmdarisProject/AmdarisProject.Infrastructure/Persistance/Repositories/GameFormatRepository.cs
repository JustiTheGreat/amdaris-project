﻿using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Application.Common.Models;
using AmdarisProject.Domain.Models;
using AmdarisProject.Infrastructure.Persistance.Contexts;
using AmdarisProject.Infrastructure.Persistance.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure.Persistance.Repositories
{
    public class GameFormatRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<GameFormat>(dbContext), IGameFormatRepository
    {
        public new async Task<GameFormat?> GetById(Guid id)
            => await _dbContext.Set<GameFormat>()
            .AsSplitQuery()
            .Include(o => o.GameType)
            .FirstOrDefaultAsync(item => item.Id.Equals(id));

        public new async Task<Tuple<IEnumerable<GameFormat>, int>> GetPaginatedData(PagedRequest pagedRequest)
            => await _dbContext.Set<GameFormat>()
            .Include(o => o.GameType)
            .CreatePaginatedResultAsync(pagedRequest);
    }
}
