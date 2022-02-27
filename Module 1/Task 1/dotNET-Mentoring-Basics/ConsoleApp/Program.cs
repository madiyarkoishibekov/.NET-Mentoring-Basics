using ClassLibrary;
using System;
namespace ConsoleApp
{
    class Program
    { 
        static void Main(string[] args)
        {
            Console.WriteLine("Hello! Please enter your name:");
            UserInfo user = new UserInfo()
            {
                UserName = Console.ReadLine()
            };

            Console.WriteLine(user.UserGreetingMessage());
        }
    }
}
