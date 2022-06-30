using FileCabinet.Infrastructure.Interfaces;
using FileCabinet.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinet.Model
{
    internal class LocalizedBook : IBook, IPrintable
    {
        public string ISBN { get; set; }
        public int NumberOfPages { get; set; }
        public string Publisher { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public DateTime DatePublished { get; set; }
    
        public string LocalPublisher { get; set; }
        public string CountryOfLocalization { get; set; }

        public void PrintDocumentProperties(IOutput output)
        {
            var card = new Card();
            card.Properties.Add("ISBN", ISBN);
            card.Properties.Add("Title", Title);
            card.Properties.Add("Authors", Authors);
            card.Properties.Add("Number of pages", NumberOfPages.ToString());
            card.Properties.Add("Original publisher", Publisher);
            card.Properties.Add("Country of localization", CountryOfLocalization);
            card.Properties.Add("Local publisher", LocalPublisher);
            card.Properties.Add("Date published", DatePublished.ToString("d"));

            output.PrintCard(card);
        }
    }
}
