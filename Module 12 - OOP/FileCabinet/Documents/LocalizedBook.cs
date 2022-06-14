using FileCabinet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinet
{
    class LocalizedBook : IPrintable, IDocument
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public int NumberOfPages { get; set; }
        public string OriginalPublisher { get; set; }
        public string CountryOfLocalization { get; set; }
        public string LocalPublisher { get; set; }
        public DateTime DatePublished { get; set; }

        public void PrintDocumentProperties(IOutput output)
        {
            var card = new Card();
            card.Properties.Add("ISBN", ISBN);
            card.Properties.Add("Title", Title);
            card.Properties.Add("Authors", Authors);
            card.Properties.Add("Number of pages", NumberOfPages.ToString());
            card.Properties.Add("Original publisher", OriginalPublisher);
            card.Properties.Add("Country of localization", CountryOfLocalization);
            card.Properties.Add("Local publisher", LocalPublisher);
            card.Properties.Add("Date published", DatePublished.ToString("d"));

            output.PrintCard(card);
        }
    }
}
