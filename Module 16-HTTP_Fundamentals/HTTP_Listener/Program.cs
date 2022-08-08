using System;

namespace HTTP_Listener
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var listener = new Listener();
            listener.StartListening("http://localhost:8888/");
        }
    }
}
