using FileCabinet.Infrastructure.Interfaces;
using FileCabinet.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FileCabinet
{
    internal class Cabinet
    {
        private IStorage _storage;
        private IOutput _output;
        private IMemoryCache _cache;
        public Cabinet(IMemoryCache memoryCache, IStorage storage = null, IOutput output = null)
        {
            _cache = memoryCache;
            _storage = storage;
            _output = output;
        }


        public IDocument SearchDocumentByNumber(int number)
        {
            var document = (IDocument)_cache.Get(number.ToString());
            if (document == null)
            {
                document = _storage.GetDocumentCardByNumber(number);
                _cache.Set(number.ToString(), document);
            }

            if (document is IPrintable printableDocument)
            {
                printableDocument.PrintDocumentProperties(_output);
            }

            return document;
        }
    }
}
