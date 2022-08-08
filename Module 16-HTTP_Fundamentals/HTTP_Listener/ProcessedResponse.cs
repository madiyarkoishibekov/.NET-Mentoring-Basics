using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HTTP_Listener
{
    internal class ProcessedResponse
    {
        public string Content { get; set; }
        public HttpListenerResponse Response { get; set; }
    }
}
