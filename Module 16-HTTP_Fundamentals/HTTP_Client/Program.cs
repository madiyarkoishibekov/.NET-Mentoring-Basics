using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HTTP_Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var httpAccess = new HttpAccess();
            var businessLogic = new BusinessLogic(httpAccess);
            var finished = false;

            do
            {
                Console.WriteLine("Please enter the number of the client task to be executed:");
                Console.WriteLine("1 - Receive MyName from a method in Listener specified as resource path.");
                Console.WriteLine("2 - Receive status messages from 5 responses.");
                Console.WriteLine("3 - Receive value of X-MyName header in response.");
                Console.WriteLine("4 - Receive value of MyName cookie in response.");
                Console.WriteLine("0 - Exit the application.");
                var result = string.Empty;
                var wrongChoice = false;

                switch (Console.ReadLine())
                {
                    case "1":
                        result = Task.Run(() => businessLogic.Task1GetMyName()).GetAwaiter().GetResult();
                        break;
                    case "2":
                        result = Task.Run(() => businessLogic.Task2GetStatusMessages()).GetAwaiter().GetResult();
                        break;
                    case "3":
                        result = Task.Run(() => businessLogic.Task3GetSpecifiedHeader()).GetAwaiter().GetResult();
                        break;
                    case "4":
                        result = Task.Run(() => businessLogic.Task4GetSpecifiedCookie()).GetAwaiter().GetResult();
                        break;
                    case "0": finished = true;
                        break; 
                    default: wrongChoice = true;
                        break;
                }

                if (!wrongChoice && !finished)
                {
                    Console.WriteLine($"Data received: {result ?? "N/A"}\n");
                }

            }
            while (!finished);
        }
    }
}
