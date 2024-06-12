using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Application.Common.Models;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Infrastructure.Persistance.Contexts;
using AmdarisProject.Infrastructure.Persistance.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure.Persistance.Repositories
{
    public abstract class GenericRepository<T>(AmdarisProjectDBContext dbContext)
        : IGenericRepository<T> where T : Model
    {
        protected readonly AmdarisProjectDBContext _dbContext = dbContext;

        public async Task<T> Create(T item)
        {
            if (item is null)
                throw new APArgumentException(nameof(item));

            if (GetById(item.Id).Result is not null)
                throw new APArgumentException(nameof(item.Id));

            T created = (await _dbContext.Set<T>().AddAsync(item)).Entity;
            return created;
        }

        public async Task<T?> GetById(Guid id)
            => await _dbContext.Set<T>().FirstOrDefaultAsync(item => item.Id.Equals(id));

        public async Task<IEnumerable<T>> GetAll()
            => await _dbContext.Set<T>().ToListAsync();

        public async Task<Tuple<IEnumerable<T>, int>> GetPaginatedData(PagedRequest pagedRequest)
            => await _dbContext.Set<T>().CreatePaginatedResultAsync(pagedRequest);

        public async Task Delete(Guid id)
            => _dbContext.Set<T>().Remove(await GetById(id)
                ?? throw new APNotFoundException(Tuple.Create(nameof(id), id)));

        public async Task<T> Update(T item)
        {
            if (item is null)
                throw new APArgumentException(nameof(item));

            T stored = await GetById(item.Id)
                ?? throw new APNotFoundException(Tuple.Create(nameof(item.Id), item.Id));

            T updated = _dbContext.Set<T>().Update(stored).Entity;
            return updated;
        }
    }
}
