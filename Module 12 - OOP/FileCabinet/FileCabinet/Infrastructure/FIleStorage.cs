using FileCabinet.Infrastructure.Interfaces;
using FileCabinet.Interfaces;
using FileCabinet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileCabinet.Infrastructure
{
    internal class FIleStorage : IStorage
    {
        public IDocument GetDocumentCardByNumber(int number)
        {
            var contentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent;
            var files = Directory.GetFiles(@$"{contentDirectory}\Content", "*.json");
            var foundFileName = files.FirstOrDefault(f => int.Parse(Path.GetFileName(f).Split("_")[1].Split(".")[0]) == number);
            var text = File.ReadAllText(foundFileName);
            var shortFileName = Path.GetFileName(foundFileName);
            var typeName = shortFileName.Split("_").First();

            IDocument document = default;
            switch (typeName.ToLower())
            {
                case "book":
                    document = JsonSerializer.Deserialize<Book>(text);
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
