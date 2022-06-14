using System;

namespace FileCabinet
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is a file cabinet app.");
            var userInput = string.Empty;
            
            // Call  paremeterless constructor gives file storage and console output by default.
            var cabinet = new Cabinet();

            while (userInput.Trim().ToLower() != "quit")
            {
                Console.WriteLine("\nPlease enter the search number or 'quit' to quit");
                userInput = Console.ReadLine();
                if (Int32.TryParse(userInput, out var documentNumber))
                {
                    cabinet.SearchDocumentByNumber(documentNumber);
                }
            }
        }
    }
}
