using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinet.Interfaces
{
    interface IStorage
    {
        IDocument GetDocumentByNumber(int number);
    }
}
