using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Client
{
    internal class HttpAccess
    {
        private readonly HttpClient _client;

        public HttpAccess()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue()
            {
                NoCache = true
            };
        }

        public async Task<string> GetResponse(string url)
        {
            string response = default;
            try
            {
                response = await _client.GetStringAsync(url);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nClient Exception Caught!");
                Console.WriteLine($"Message: {e.Message}");
            }

            return response;
        }

        public async Task<string> GetStatus(string url)
        {
            string statusMessage = default;
            try
            {
                using var response = await _client.GetAsync(url);
                statusMessage = $"{(int)response.StatusCode} - {response.StatusCode.ToString()}";
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nClient Exception Caught!");
                Console.WriteLine($"Message :{e.Message}");
            }

            return statusMessage;
        }

        public async Task<string> GetHeader(string url, string headerName)
        {
            string headerValue = default;
            try
            {
                var response = await _client.GetAsync(url);
                if (response.Headers.TryGetValues(headerName, out var headerValues))
                {
                    headerValue = string.Join(",", headerValues);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nClient Exception Caught!");
                Console.WriteLine($"Message :{e.Message}");
            }

            return headerValue;
        }

        public async Task<string> GetValueFromCookie(string url, string cookieName)
        {
            string cookieValue = default;
            using var handler = new SocketsHttpHandler();
            using var clientWithCookies = new HttpClient(handler);
            try
            {
                var response = await clientWithCookies.GetAsync(url);
                cookieValue = handler.CookieContainer.GetCookies(new Uri(url))[cookieName]?.Value;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nClient Exception Caught!");
                Console.WriteLine($"Message :{e.Message}");
            }

            return cookieValue;
        }

    }
}
