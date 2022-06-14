using FileCabinet.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace FileCabinet.Repository
{
    class FileStorage : IStorage
    {
        public IDocument GetDocumentByNumber(int number)
        {
            var files = Directory.GetFiles($"{Directory.GetCurrentDirectory()}\\Content", "*.json");
            var foundFileName = files.FirstOrDefault(f => Int32.Parse(Path.GetFileName(f).Split("_")[1].Split(".")[0]) == number);
            var text = File.ReadAllText(foundFileName);
            var shortFileName = Path.GetFileName(foundFileName);
            var typeName = shortFileName.Substring(0, shortFileName.IndexOf("_"));

            IDocument document = default;
            switch (typeName.ToLower())
            {
                case "book": document = JsonSerializer.Deserialize<Book>(text);
                    break;
                case "locbook":
                    document = JsonSerializer.Deserialize<LocalizedBook>(text);
                    break;
                case "patent":
                    document = JsonSerializer.Deserialize<Patent>(text);
                    break;
                case "magazine":
                    document = JsonSerializer.Deserialize<Magazine>(text);
                    break;
            }

            return document;
        }
    }
}
