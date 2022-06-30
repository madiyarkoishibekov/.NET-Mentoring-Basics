using FileCabinet.Infrastructure.Interfaces;
using FileCabinet.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinet.Model
{
    internal class Book : IBook, IPrintable
    {
        public string ISBN { get; set; }
        public int NumberOfPages { get; set; }
        public string Publisher { get; set; }
        public string Title { get ; set; }
        public string Authors { get; set; }
        public DateTime DatePublished { get; set; }

        public void PrintDocumentProperties(IOutput output)
        {
            var card = new Card();
            card.Properties.Add("ISBN", ISBN);
            card.Properties.Add("Title", Title);
            card.Properties.Add("Authors", Authors);
            card.Properties.Add("Date published", DatePublished.ToString("d"));
            card.Properties.Add("Number of pages", NumberOfPages.ToString());
            card.Properties.Add("Publisher", Publisher);

            output.PrintCard(card);
        }
    }
}
