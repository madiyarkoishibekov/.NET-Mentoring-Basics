using FileCabinet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinet
{
    class Magazine : IPrintable, IDocument
    {
        public string Title { get; set; }
        public string Publisher { get; set; }
        public int ReleaseNumber { get; set; }
        public DateTime PublishedDate { get; set; }

        public void PrintDocumentProperties(IOutput output)
        {
            var card = new Card();
            card.Properties.Add("Title", Title);
            card.Properties.Add("Publisher", Publisher);
            card.Properties.Add("Release number", ReleaseNumber.ToString());
            card.Properties.Add("Published date", PublishedDate.ToString("d"));

            output.PrintCard(card);
        }
    }
}
