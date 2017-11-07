using System;
using System.Collections.Generic;

namespace Notes.Model
{
    public class Model: Repository<Note>, IObservable
    {
        DataBase<Note> db = new DataBase<Note>();

        private List<IObserver> observers;
        
        public Model()
        {
            observers = new List<IObserver>();

            if (db.ReadFromDB() != null)
            {
                DbSynchronize(db.ReadFromDB());
            }          
        }
        
        public void RegisterObserver(IObserver newObserver)
        {
            observers.Add(newObserver);
        }

        public void RemoveObserver(IObserver currentObserver)
        {
            observers.Remove(currentObserver);
        }

        public void NotifyObservers(string answer)
        {
            List<Note> temp = new List<Note>();

            if (answer == "ok")
            {
                temp = GetAll();
            }
            else
            {
                temp = null;
            }

            foreach (IObserver item in observers)
            {
                item.Update(temp,answer);
            }
        }

        
        protected override void UpdateElement(Note element)
        {
            Delete(element); 
            Create(element);
        }

        public void ToDo(string task, Note newNote)
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
                    case "Change":
                        UpdateElement(newNote);  // update element in local collection
                        break;
                    default:
                        NotifyObservers("ok");
                        return;
                }

                db.WriteToDB(GetAll());         // wite collection to DB
                DbSynchronize(db.ReadFromDB()); // update local collection
                NotifyObservers("ok");
            }
            catch (Exception ex)
            {
                NotifyObservers(ex.Message); ;
            }
        }
    }
}
