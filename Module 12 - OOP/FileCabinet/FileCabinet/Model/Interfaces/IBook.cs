using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinet.Interfaces
{
    internal interface IBook : IDocument
    {
        public string ISBN { get; set; }
        public int NumberOfPages { get; set; }  
        public string Publisher { get; set; }   
    }
}
