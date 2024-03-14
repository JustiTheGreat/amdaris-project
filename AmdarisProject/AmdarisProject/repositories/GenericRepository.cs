using AmdarisProject.models;
using AmdarisProject.utils;

namespace AmdarisProject.repositories
{
    public class GenericRepository<T> where T : Model
    {
        private readonly List<T> _dataSet = [];

        public T GetById(int id)
        {
            return _dataSet.First(item => item.Id == id)
                ?? throw new NullReferenceException(MessageFormatter.Format(nameof(GenericRepository<T>), nameof(GetById), "not found"));

        }

        public IEnumerable<T> GetAll()
        {
            return _dataSet;
        }

        public void Add(T item)
        {
            item = item
                ?? throw new ArgumentNullException(MessageFormatter.Format(nameof(GenericRepository<T>), nameof(Add), "null value"));
            _dataSet.Add(item);
        }

        public void Update(T item)
        {
            item = item
                ?? throw new ArgumentNullException(MessageFormatter.Format(nameof(GenericRepository<T>), nameof(Add), "null value"));
            T storedItem = GetById(item.Id);
            storedItem.CopyDataFrom(item);
        }

        public void Delete(int id)
        {
            _dataSet.Remove(GetById(id));
        }
    }
}
