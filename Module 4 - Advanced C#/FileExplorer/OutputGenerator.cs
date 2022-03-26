using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer
{
    class OutputGenerator
    {
        public void WriteFileItem(string fileItem)
        {
            Console.WriteLine(fileItem.ToUpper());
        }

        public void WriteHeader(string header, bool newLine)
        {
            if (newLine)
            {
                Console.WriteLine();
            }
            Console.WriteLine(header);
            Console.WriteLine(String.Empty.PadLeft(header.Length, '='));
        }

        public void WriteEventMessage(string message)
        {
            Console.WriteLine($"Event received: {message}");
        }
    }
}
