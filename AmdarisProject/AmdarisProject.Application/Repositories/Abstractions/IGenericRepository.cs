using AmdarisProject.models;

namespace AmdarisProject.repositories.abstractions
{
    public interface IGenericRepository<T> where T : Model
    {
        T Create(T item);
        T GetById(ulong id);
        IEnumerable<T> GetByIds(IEnumerable<ulong> ids);
        IEnumerable<T> GetAll();
        void Delete(ulong id);
        T Update(T item);
    }
}