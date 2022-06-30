using FileCabinet.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinet.Infrastructure
{
    internal class ConsoleOutput : IOutput
    {
        public void PrintCard(Card card)
        {
            foreach (var kvp in card.Properties)
            {
                Console.WriteLine($"{kvp.Key}\t{kvp.Value}");
            }
        }
    }
}
