using FileCabinet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinet.Output
{
    class ConsoleOutput : IOutput
    {
        public bool PrintCard(Card card)
        {
            foreach(var kvp in card.Properties)
            {
                Console.WriteLine($"{kvp.Key, -30}{kvp.Value, -30}"); 
            }

            return true;
        }
    }
}
