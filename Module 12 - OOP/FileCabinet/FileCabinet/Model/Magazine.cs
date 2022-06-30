using FileCabinet.Infrastructure.Interfaces;
using FileCabinet.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinet.Model
{
    internal class Magazine : IDocument, IPrintable
    {
        private string _publisher;
        public string Title { get; set; }
        public string Authors { get { return _publisher; } set { _publisher = value; }}
        public DateTime DatePublished { get; set; }

        public int ReleaseNumber { get; set; }

        public void PrintDocumentProperties(IOutput output)
        {
            var card = new Card();
            card.Properties.Add("Title", Title);
            card.Properties.Add("Publisher", Authors);
            card.Properties.Add("Release number", ReleaseNumber.ToString());
            card.Properties.Add("Published date", DatePublished.ToString("d"));

            output.PrintCard(card);
        }
    }
}
