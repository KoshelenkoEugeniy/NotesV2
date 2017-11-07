using System;
using System.Collections.Generic;
using Notes.Classes;
using Notes.Model;

namespace Notes
{
    class Program
    {
        static View.View blView = new View.View();

        static void Main(string[] args)
        {
            View.View blView = new View.View();

            Model.Model blModel = new Model.Model();

            Observer observer = new Observer(blModel);

            Note newNote;

            List<Note> temp = new List<Note>();

            string userAnswer = ""; // result of user's choice from Menu

            string noteText; // the text of new note

            int numberOfNote; // note's number

            string modelAnswer = "";


            while (userAnswer != "E" && userAnswer != "e") // Exit
            {

                userAnswer = WriteInfo_ReadAnswer(Menu(0));

                switch (userAnswer)
                {
                    case "C":
                    case "c":

                        noteText = WriteInfo_ReadAnswer(Menu(4)); // get text of new note

                        newNote = new Note(noteText);

                        blModel.ToDo("Create", newNote);

                        CheckModelAnswer(observer.status); // checking if were errors during process of addition new element to DB 

                        break;

                    case "D":
                    case "d":
                        temp = observer.GetAll();

                        noteText = WriteInfo_ReadAnswer(Menu(1)); // get string number of element that should be deleted

                        numberOfNote = checkNoteNumber(noteText, temp); // checking if string number is int and it is in the range of collection

                        if (numberOfNote != -123) // if number = -123 than was typed wrong note's number 
                        {
                            blModel.ToDo("Delete", temp[numberOfNote]);
                        }
                        else
                        {
                            blView.ShowInfo(Menu(8));
                            break;
                        }

                        CheckModelAnswer(observer.status); // checking if were errors during process of addition new element to DB 

                        break;


                    case "R":
                    case "r":

                        blModel.ToDo("Read", null);

                        temp = observer.GetAll();

                        noteText = WriteInfo_ReadAnswer(Menu(7));

                        numberOfNote = checkNoteNumber(noteText, temp);

                        if (numberOfNote != -123)
                        {
                            if (temp.Count > 0) // if DB is not empty
                            {
                                for (int i = 0; i < temp.Count; i++)
                                {
                                    temp[i].Id = i;
                                }                       // number each note

                                if (numberOfNote == -1)
                                {
                                    modelAnswer = Convert(temp); // show all notes
                                }
                                else
                                {
                                    newNote = temp[numberOfNote];
                                    List<Note> tempSelected = new List<Note>(); // show selected note
                                    tempSelected.Add(newNote);
                                    modelAnswer = Convert(tempSelected);
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

                        temp = observer.GetAll();

                        noteText = WriteInfo_ReadAnswer(Menu(1));

                        numberOfNote = checkNoteNumber(noteText,temp);

                        if (numberOfNote != -123)
                        {
                            noteText = WriteInfo_ReadAnswer(Menu(5, temp[numberOfNote].Status));

                            if (noteText == "Y" || noteText == "y") // checking if user wants to change the status
                            {
                                if (temp[numberOfNote].Status == "Current")
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
                                newStatus = temp[numberOfNote].Status;
                            }


                            noteText = WriteInfo_ReadAnswer(Menu(6, temp[numberOfNote].Title));

                            if (noteText == "Y" || noteText == "y") // checking if user wants to change the note's text
                            {
                                newText = WriteInfo_ReadAnswer(Menu(4));

                                shouldChange = true;
                            }
                            else
                            {
                                if (shouldChange == true) newText = temp[numberOfNote].Title;
                            }

                            if (shouldChange == true)
                            {
                                newNote = new Note(newText, newStatus, numberOfNote);

                                blModel.ToDo("Change", newNote);

                                CheckModelAnswer(observer.status);
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
        }

        

        // Method convert to string all list's elements

        public static string Convert(List<Note> collection)
        {
            string outputText = "";

            foreach (var element in collection)
            {
                if (outputText == "")
                {
                    outputText =
                        $"\r \n Note: {element.Id}         Created: {element.DateOfCreation.ToString(@"dd\/MM\/yyyy HH\:mm\:ss tt")}       Status: {element.Status} \n \n {element.Title} \n";
                }
                else
                {
                    outputText = outputText +
                                 $"\r \n Note: {element.Id}         Created: {element.DateOfCreation.ToString(@"dd\/MM\/yyyy HH\:mm\:ss tt")}       Status: {element.Status} \n \n {element.Title} \n";
                }
            }
            return outputText;
        }

        // Menu

        public static string Menu(int number, string additionalInfo = "")
        {
            switch (number)
            {
                case 0:
                    return
                        "\r \n Please choose necessary command to be applied to notes: \r \n C: create   D: delete    R: read     U: update     E: exit \r \n";
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

        // method shows some text and waiting on user's choice

        public static string WriteInfo_ReadAnswer(string text)
        {
            blView.ShowInfo(text);
            return blView.GetInfo();
        }

        // method checks input info from user if it is a number

        public static int checkNoteNumber(string number, List<Note> list)
        {
            int intNumber;

            try
            {
                intNumber = int.Parse(number);

                if (list.Count <= intNumber || intNumber < -1
                ) // cheking if an input number in the range of note's list 
                {
                    intNumber = -123; // if not -123 returns
                }
            }
            catch
            {
                intNumber = -123;
            }

            return intNumber;
        }


        // method checks model's result of add/delete/update operations: ok or error

        public static void CheckModelAnswer(string answer)
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
    }
}