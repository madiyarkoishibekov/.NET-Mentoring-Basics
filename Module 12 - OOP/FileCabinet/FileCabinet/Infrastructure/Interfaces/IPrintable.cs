using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinet.Infrastructure.Interfaces
{
    internal interface IPrintable
    {
        public void PrintDocumentProperties(IOutput output);
    }
}
