using FileCabinet;
using FileCabinet.Infrastructure;
using FileCabinet.Interfaces;
using Microsoft.Extensions.Caching.Memory;

Console.WriteLine("This is a file cabinet app.");
var userInput = string.Empty;

var cabinet = new Cabinet(new MemoryCache(new MemoryCacheOptions()),new FIleStorage(), new ConsoleOutput());

while (userInput.Trim().ToLower() != "quit")
{
    Console.WriteLine("\nPlease enter the search number or 'quit' to quit");
    userInput = Console.ReadLine();
    if (Int32.TryParse(userInput, out var documentNumber))
    {
        cabinet.SearchDocumentByNumber(documentNumber);
    }
}