using System;
using System.Collections.Generic;
using Notes.Model;

namespace Notes
{
    class Program
    {
        static void Main(string[] args)
        {
            View.View blView = new View.View();

            Model.Model blModel = new Model.Model();

            Note newNote;

            List<Note> dbCopy = new List<Note>();

            string userAnswer = "";                 // result of user's choice from Menu

            string noteText;                        // the text of new note

            int numberOfNote;                       // note's number

            while (userAnswer != "E" && userAnswer != "e")  // Exit
            {

                userAnswer = WriteInfo_ReadAnswer(Menu(0));

                switch (userAnswer)
                {
                    case "C":
                    case "c":

                        noteText = WriteInfo_ReadAnswer(Menu(4));   // get text of new note

                        newNote = new Note(noteText);

                        blModel.ToDo("Create", newNote, out List<Note> newCollection, out String modelAnswer);

                        if (newCollection != null)  // if element was successfully added to DB than synchronizing collections
                        {
                            dbCopy = newCollection;
                        }
                            
                        CheckModelAnswer(modelAnswer);  // checking if were errors during process of addition new element to DB 

                        break;

                    case "D":
                    case "d":

                        noteText = WriteInfo_ReadAnswer(Menu(1));   // get string number of element that should be deleted

                        numberOfNote = checkNoteNumber(noteText);   // checking if string number is int and it is in the range of collection

                        if (numberOfNote != -123)                   // if number = -123 than was typed wrong note's number 
                        {
                            blModel.ToDo("Delete", dbCopy[numberOfNote], out newCollection, out modelAnswer);

                            if (newCollection != null)
                            {
                                dbCopy = newCollection; // if element was successfully deleted from DB than synchronizing collections
                            }
                        }
                        else
                        {
                            blView.ShowInfo(Menu(8));
                            break;
                        }

                        CheckModelAnswer(modelAnswer); // checking if were errors during process of addition new element to DB 

                        break;


                    case "R":
                    case "r":

                        if (dbCopy.Count == 0)  // if local copy of DB wasn't yet synchronized with main DB
                        {
                            dbCopy = blModel.FirstSynchronize();    // doing synchonizing
                        }
                        
                        noteText = WriteInfo_ReadAnswer(Menu(7));
                        
                        numberOfNote = checkNoteNumber(noteText);

                        if (numberOfNote != -123)
                        {
                            if (dbCopy.Count > 0)   // if DB is not empty
                            {
                                SetNumbers();       // number each note

                                if (numberOfNote == -1)
                                {
                                    modelAnswer = Convert(dbCopy);  // show all notes
                                }
                                else
                                {
                                    newNote = dbCopy[numberOfNote];
                                    List<Note> temp = new List<Note>(); // show selected note
                                    temp.Add(newNote);
                                    modelAnswer = Convert(temp);   
                                }

                                blView.ShowInfo(modelAnswer);
                            }  
                        }
                        else
                        {
                            blView.ShowInfo(Menu(8));
                        }
                        break;


                    case "U":
                    case "u":

                        string newStatus = "";

                        string newText = "";

                        bool shouldChange = false;


                        noteText = WriteInfo_ReadAnswer(Menu(1));

                        numberOfNote = checkNoteNumber(noteText);

                        if (numberOfNote != -123)
                        {
                            noteText = WriteInfo_ReadAnswer(Menu(5, dbCopy[numberOfNote].Status));

                            if (noteText == "Y" || noteText == "y")         // checking if user wants to change the status
                            {
                                if (dbCopy[numberOfNote].Status == "Current")
                                {
                                    newStatus = "Finished";
                                }
                                else
                                {
                                    newStatus = "Current";
                                }

                                shouldChange = true;
                            }
                            else
                            {
                                newStatus = dbCopy[numberOfNote].Status;
                            }


                            noteText = WriteInfo_ReadAnswer(Menu(6, dbCopy[numberOfNote].Title));

                            if (noteText == "Y" || noteText == "y") // checking if user wants to change the note's text
                            {
                                newText = WriteInfo_ReadAnswer(Menu(4));

                                shouldChange = true;
                            }
                            else
                            {
                                if (shouldChange == true) newText = dbCopy[numberOfNote].Title;
                            }

                            if (shouldChange == true)
                            {
                                newNote = new Note(newText, newStatus, numberOfNote);

                                blModel.ToDo("Change", newNote, out newCollection, out modelAnswer);

                                if (newCollection != null)
                                {
                                    dbCopy = newCollection;
                                }

                                CheckModelAnswer(modelAnswer);

                            }
                        }
                        else
                        {
                            blView.ShowInfo(Menu(8));
                        }
                        break;

                    default:
                        blView.ShowInfo(Menu(-1));
                        break;
                }
            }


            // method numbers each note before showing

            void SetNumbers()
            {
                for (int i = 0; i < dbCopy.Count; i++)
                {
                    dbCopy[i].Id = i;
                }
            }


            // method checks input info from user if it is a number

            int checkNoteNumber(string number)
            {
                int intNumber;

                try
                {
                    intNumber = int.Parse(number);

                    if (dbCopy.Count <= intNumber || intNumber < -1) // cheking if an input number in the range of note's list 
                    {
                        intNumber = -123;                           // if not -123 returns
                    }
                }
                catch
                {
                    intNumber = -123;
                }

                return intNumber;
            }


            // method checks model's result of add/delete/update operations: ok or error

            void CheckModelAnswer(string answer)
            {
                if (answer == "ok")
                {
                    blView.ShowInfo(Menu(3));
                }
                else
                {
                    blView.ShowInfo(answer);
                }
            }


            // method shows some text and waiting on user's choice

            string WriteInfo_ReadAnswer(string text)
            {
                blView.ShowInfo(text);
                return blView.GetInfo();
            }


            // Menu

            string Menu(int number, string additionalInfo = "")
            {
                switch (number)
                {
                    case 0:
                        return "\r \n Please choose necessary command to be applied to notes: \r \n C: create   D: delete    R: read     U: update     E: exit \r \n";
                    case 1:
                        return "\r \n Please type a note's number: \r \n";
                    case 2:
                        return "\r \n Please type a key word, that should be found: \r \n";
                    case 3:
                        return "\r \n Operation successfully done. \r \n";
                    case 4:
                        return "\r \n Please type a note: \r \n";
                    case 5:
                        return $"\r \n Current status is: {additionalInfo}. Should the status be changed? Y/N: \r \n";
                    case 6:
                        return $"\r \n Current note is: {additionalInfo}. Should the note be changed? Y/N: \r \n";
                    case 7:
                        return "\r \n Please type a note's number. If type -1 all notes will be shown. \r \n";
                    case 8:
                        return "\r \n Wrong number. Please retype... \r \n";
                    default:
                        return "\r \n Wrong command. Please retype... \r \n";
                }
            }


            // Method convert to string all list's elements

            string Convert(List<Note> collection)
            {
                string outputText = "";

                foreach (var element in collection)
                {
                    if (outputText == "")
                    {
                        outputText = $"\r \n Note: {element.Id}         Created: {element.DateOfCreation.ToString(@"dd\/MM\/yyyy HH\:mm\:ss tt")}       Status: {element.Status} \n \n {element.Title} \n";
                    }
                    else
                    {
                        outputText = outputText + $"\r \n Note: {element.Id}         Created: {element.DateOfCreation.ToString(@"dd\/MM\/yyyy HH\:mm\:ss tt")}       Status: {element.Status} \n \n {element.Title} \n"; 
                    }
                }
                return outputText;
            }

        }
    }
}