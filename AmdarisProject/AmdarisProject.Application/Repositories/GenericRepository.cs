using AmdarisProject.models;
using AmdarisProject.repositories.abstractions;
using Domain.Exceptions;

namespace AmdarisProject.repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : Model
    {
        protected readonly List<T> _dataSet = [];

        public T Create(T item)
        {
            if (item is null)
                throw new APArgumentException(nameof(GenericRepository<T>), nameof(Create), nameof(item));

            _dataSet.Add(item);
            return GetById(item.Id);
        }

        public T GetById(ulong id)
            => _dataSet.First(item => item.Id == id)
                ?? throw new APNotFoundException(nameof(GenericRepository<T>), nameof(GetById), nameof(T));

        public IEnumerable<T> GetByIds(IEnumerable<ulong> ids)
            => ids.Select(GetById).ToList();

        public IEnumerable<T> GetAll()
            => _dataSet;

        public void Delete(ulong id)
            => _dataSet.Remove(GetById(id));

        public abstract T Update(T item);
    }
}
