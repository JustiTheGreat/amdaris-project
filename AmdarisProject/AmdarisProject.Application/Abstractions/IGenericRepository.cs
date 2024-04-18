namespace AmdarisProject.Application.Abstractions
{
    public interface IGenericRepository<T>
    {
        Task<T> Create(T item);
        Task<T?> GetById(Guid id);
        Task<IEnumerable<T>> GetByIds(IEnumerable<Guid> ids);
        Task<IEnumerable<T>> GetAll();
        Task Delete(Guid id);
        Task<T> Update(T item);
    }
}