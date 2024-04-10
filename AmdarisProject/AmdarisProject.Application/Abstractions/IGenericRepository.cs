namespace AmdarisProject.Application.Abstractions
{
    public interface IGenericRepository<T>
    {
        Task<T> Create(T item);
        Task<T?> GetById(ulong id);
        Task<IEnumerable<T>> GetByIds(IEnumerable<ulong> ids);
        Task<IEnumerable<T>> GetAll();
        Task Delete(ulong id);
        Task<T> Update(T item);
    }
}