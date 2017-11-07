using System.Collections.Generic;
using Notes.Model;

namespace Notes.Classes
{
    public class Observer : IObserver
    {
        private IObservable _model;

        private List<Note> _dbCopy = new List<Note>();

        public string status { get; set; }

        public Observer(IObservable target)
        {
            _model = target;
            _model.RegisterObserver(this);
        }

        public void Update(object myObject, string answer)
        {
            if (answer == "ok")
            {
                _dbCopy = (List<Note>)myObject;
            }
            status = "";
            status = answer;
        }

        public void Unsubscribe()
        {
            _model.RemoveObserver(this);
            _model = null;
        }

        public List<Note> GetAll()
        {
            return _dbCopy;
        }
    }
}


