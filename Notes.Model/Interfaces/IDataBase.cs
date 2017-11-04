using System.Collections.Generic;

namespace Notes.Model
{
    public interface IDataBase<T>
    {
        void WriteToDB(List<T> collection);

        List<T> ReadFromDB();
    }
}