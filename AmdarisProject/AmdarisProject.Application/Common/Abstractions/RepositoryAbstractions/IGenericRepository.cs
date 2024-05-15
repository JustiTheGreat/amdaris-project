using OnlineBookShop.Application.Common.Models;

namespace AmdarisProject.Application.Common.Abstractions.RepositoryAbstractions
{
    public interface IGenericRepository<T>
    {
        Task<T> Create(T item);
        Task<T?> GetById(Guid id);
        Task<IEnumerable<T>> GetByIds(IEnumerable<Guid> ids);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetPagedData(PagedRequest pagedRequest);
        Task Delete(Guid id);
        Task<T> Update(T item);
    }
}