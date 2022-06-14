using FileCabinet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinet
{
    class Patent : IPrintable, IDocument
    {
        public string Title { get; set; }
        public string Authors { get; set; }
        public DateTime DatePublished { get; set; }
        public DateTime ExpirationDate{ get; set; }
        public string UniqueId { get; set; }

        public void PrintDocumentProperties(IOutput output)
        {
            var card = new Card();
            card.Properties.Add("Title", Title);
            card.Properties.Add("Authors", Authors);
            card.Properties.Add("Date published", DatePublished.ToString("d"));
            card.Properties.Add("Expiration date", ExpirationDate.ToString("d"));
            card.Properties.Add("UniqueId", UniqueId);

            output.PrintCard(card);
        }
    }
}
