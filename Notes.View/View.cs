using System;

namespace Notes.View
{
    public class View
    {
        public void ShowInfo(string text)
        {
            Console.WriteLine($"{text}");
        }

        public string GetInfo()
        {
            return Console.ReadLine();
        }
    }
}
