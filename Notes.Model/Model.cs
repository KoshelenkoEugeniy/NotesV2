using System;
using System.Collections.Generic;

namespace Notes.Model
{
    public class Model: Repository<Note>
    {
        DataBase<Note> db = new DataBase<Note>();

        public Model()
        {
            if (db.ReadFromDB() != null)
            {
                DbSynchronize(db.ReadFromDB()); 
            }         
        }

        public List<Note> FirstSynchronize()
        {
            return GetAll();
        }

        protected override void UpdateElement(Note element)
        {
            Delete(element); 
            Create(element);
        }

        public void ToDo(string task, Note newNote, out List<Note> updatedCollection, out String modelAnswer)
        {
            try
            {
                switch (task)
                {
                    case "Create":
                        Create(newNote);    // add element to local collection
                        break;
                    case "Delete":
                        Delete(newNote);    // delete element from local collection
                        break;
                    default:
                        UpdateElement(newNote);  // update element in local collection
                        break;
                }

                db.WriteToDB(GetAll());         // wite collection to DB
                DbSynchronize(db.ReadFromDB()); // update local collection
                updatedCollection = GetAll();   // return updated collection
                modelAnswer = "ok";
            }
            catch (Exception ex)
            {
                modelAnswer =  ex.Message;
                updatedCollection = null;
            }
        }
    }
}
