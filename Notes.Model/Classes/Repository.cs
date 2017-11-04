using System.Collections.Generic;

namespace Notes.Model
{
    public abstract class Repository<T> : IRepository<T> where T : IObjectId
    {
        private List<T> _collection = new List<T>();

        protected abstract void UpdateElement(T element);

        public void Create(T newElement)
        {
            _collection.Add(newElement);
        }

        public void Delete(IObjectId id)
        {
            _collection.RemoveAt(id.Id);
        }

        public void Update(T element)
        {
            UpdateElement(element);
        }

        public List<T> GetAll()
        {
            return _collection;
        }

        public T Get(IObjectId id)
        {
            return _collection[id.Id];
        }

        public void DbSynchronize(List<T> collection)   // method synchronize local collection with db
        {
            _collection = collection;
        }
    }
}