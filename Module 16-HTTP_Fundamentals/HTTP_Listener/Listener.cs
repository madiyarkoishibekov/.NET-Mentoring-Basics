using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace HTTP_Listener
{
    public class Listener
    {
        public void StartListening(string prefix)
        {
            var listener = new HttpListener();
            try
            {
                listener.Prefixes.Add(prefix);

                listener.Start();
                Console.WriteLine("I will respond to 10 requests and close the window.");
                Console.WriteLine("Start listening on http://localhost:8888/....\n");

                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"Listening to request #{i + 1} started");
                    var result = listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
                    result.AsyncWaitHandle.WaitOne();
                    Thread.Sleep(300); // time to send response, otherwise listener gets disposed without sending
                    Console.WriteLine($"Request #{i + 1} is processed.\n");
                }
            }
            finally
            {
                listener.Close();
            }
        }

        public void ListenerCallback(IAsyncResult result)
        {
            var listener = (HttpListener)result.AsyncState;
            var context = listener.EndGetContext(result);
            var request = context.Request;
            var response = context.Response;
            Console.WriteLine($"Received request string: {request.Url.OriginalString}");

            var responseString = ProcessRequest(request, response);
            response.AppendHeader("Cache-Control", "no-cache");
            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            var stringMessage = string.IsNullOrEmpty(responseString) ? "(data is in headers or cookies)" : responseString;
            Console.WriteLine($"Sending data: {stringMessage}");
            output.Close();
        }

        private string ProcessRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            var content = string.Empty;
            switch (request.Url.PathAndQuery)
            {
                case "/MyName/":
                    content = "My name is Giovanni Giorgio";
                    break;
                case "/Information/":
                    // Setting this status code hangs the Client.
                    // response.StatusCode = 100;
                    response.StatusDescription = "Information";
                    break;
                case "/Success/":
                    response.StatusCode = 200;
                    response.StatusDescription = "Success";
                    break;
                case "/Redirection/":
                    response.StatusCode = 300;
                    response.StatusDescription = "Redirection";
                    break;
                case "/ClientError/":
                    response.StatusCode = 400;
                    response.StatusDescription = "ClientError";
                    break;
                case "ServerError/":
                    response.StatusCode = 500;
                    response.StatusDescription = "ServerError";
                    break;
                case "/MyNameByHeader/":
                    response.Headers.Add("X-MyName", "My name is Giovanni Giorgio");
                    break;
                case "/MyNameByCookies/":
                    response.AppendCookie(new Cookie("MyName", "My name is Giovanni Giorgio"));
                    break;
            }

            return content;
        }
    }
}
