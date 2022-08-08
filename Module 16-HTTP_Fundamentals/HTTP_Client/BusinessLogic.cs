using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HTTP_Client
{
    internal class BusinessLogic
    {
        private HttpAccess _httpAccess;
        public BusinessLogic(HttpAccess httpAccess)
        {
            _httpAccess = httpAccess;
        }

        public async Task<string> Task1GetMyName()
        {
            var url = "http://localhost:8888/MyName/";
            var myName = await _httpAccess.GetResponse(url);
            return myName;
        }

        public async Task<string> Task2GetStatusMessages()
        {
            var urls = new List<string>()
            {
                "http://localhost:8888/Information/",
                "http://localhost:8888/Success/",
                "http://localhost:8888/Redirection/",
                "http://localhost:8888/ClientError/",
                "http://localhost:8888/ServerError/"
            };
            var statusMessages = new List<string>();
            foreach (var url in urls)
            {
                Console.WriteLine($"Requesting {url}");
                var statusMessage = await _httpAccess.GetStatus(url);
                Console.WriteLine($"Status code received {statusMessage}");
                statusMessages.Add(statusMessage);
            }

            return string.Join(",", statusMessages);
        }

        public async Task<string> Task3GetSpecifiedHeader()
        {
            var url = "http://localhost:8888/MyNameByHeader/";
            var headerName = "X-MyName";
            var myName = await _httpAccess.GetHeader(url, headerName);
            return myName;
        }

        public async Task<string> Task4GetSpecifiedCookie()
        {
            var url = " http://localhost:8888/MyNameByCookies/";
            var cookieName = "MyName";
            var myName = await _httpAccess.GetValueFromCookie(url, cookieName);
            return myName;
        }

    }
}
