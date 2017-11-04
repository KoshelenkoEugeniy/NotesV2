using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Notes.Model
{
    public class DataBase<T> : IDataBase<T>
    {
        private string dbName = "../localDB/DataBase.json";

        public void WriteToDB(List<T> collection)
        {
            collection.Sort();
            File.WriteAllText(dbName, JsonConvert.SerializeObject(collection));
        }

        public List<T> ReadFromDB()
        {
            try
            {
                return JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(dbName));
            }
            catch (Exception ex)
            {
                string text = ex.Message;
                return new List<T>();
            }
            
        }
    }
}