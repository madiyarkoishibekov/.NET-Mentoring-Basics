using FileCabinet.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinet.Infrastructure.Interfaces
{
    internal interface IStorage
    {
        public IDocument GetDocumentCardByNumber(int number); 
    }
}
