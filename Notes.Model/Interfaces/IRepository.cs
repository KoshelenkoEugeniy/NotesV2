using System.Collections.Generic;

namespace Notes.Model
{
    public interface IRepository<T>
    {
        void Create(T newElement);

        void Delete(IObjectId id);

        void Update(T element);

        List<T> GetAll();

        T Get(IObjectId id);
    }
}