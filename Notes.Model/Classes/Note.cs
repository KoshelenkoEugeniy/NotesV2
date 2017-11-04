using System;

namespace Notes.Model
{
    public class Note: IObjectId, IComparable<Note>
    {
        public DateTime DateOfCreation { get; set; }

        public string Title { get; set; }

        public String Status { get; set; }

        public int Id { get; set; }

        public Note()
        {
            DateOfCreation = DateTime.MinValue ;
            Title = "";
            Status = "";
        }

        public Note(String title)
        {
            DateOfCreation = DateTime.Now;
            Title = title;
            Status = "Current";
        }

        public Note(String title, string status, int id)
        {
            DateOfCreation = DateTime.Now;
            Title = title;
            Status = status;
            Id = id;
        }

        public int CompareTo(Note otherNote)
        {
            return this.DateOfCreation.CompareTo(otherNote.DateOfCreation);
        }  
    }
}
