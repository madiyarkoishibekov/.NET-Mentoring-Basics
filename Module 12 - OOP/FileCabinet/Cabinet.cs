using FileCabinet.Interfaces;
using FileCabinet.Output;
using FileCabinet.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Caching;

namespace FileCabinet
{
    class Cabinet
    {
        private IStorage _storage;
        private IOutput _output;
        private MemoryCache _cache = MemoryCache.Default;

        public Cabinet(IStorage storage = null, IOutput output = null)
        {
            _storage = storage == null ? new FileStorage() : storage;
            _output = output == null ? new ConsoleOutput() : output;
        }

        public IDocument SearchDocumentByNumber(int number)
        {
            var document = (IDocument)_cache.Get(number.ToString());
            if (document == null)
            {
                document = _storage.GetDocumentByNumber(number);
                // Add value to cache for 10 seconds for easy testing.
                _cache.Add(new CacheItem(number.ToString(), document), 
                    new CacheItemPolicy() { SlidingExpiration = new TimeSpan(0, 0, 10) });
            }
            
            if (document is IPrintable printableDocument)
            {
                printableDocument.PrintDocumentProperties(_output);
            }

            return document;
        }
    }
}
