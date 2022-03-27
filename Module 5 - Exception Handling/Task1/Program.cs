using System;

namespace Task1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Just enter the word to see first character as an output.");
            Console.WriteLine("Press Esc to exit application.");


            while (true)
            {
                string line = "";
                ConsoleKeyInfo info = Console.ReadKey();
                ConsoleKey firstKey = info.Key;
                if (firstKey == ConsoleKey.Escape)
                {
                    break;
                }

                if (firstKey != ConsoleKey.Enter)
                {
                    char firstLetter = info.KeyChar;
                    line = firstLetter.ToString() + Console.ReadLine();
                }

                PrintFirstCharacter(line);
            }
        }

        private static void PrintFirstCharacter(string line)
        {
            char a;
            try
            {
                a = line[0];
                Console.WriteLine($"First character of the word {line} is {a}");
            }
            catch
            {
                Console.WriteLine("You must be entering empty line. Try again.");
            }
        }
    }
}