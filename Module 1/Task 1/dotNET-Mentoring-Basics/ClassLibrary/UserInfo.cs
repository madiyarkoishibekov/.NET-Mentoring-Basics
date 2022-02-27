using System;

namespace ClassLibrary
{
    public class UserInfo
    {
        public string UserName { get; set; }
        public string UserGreetingMessage()
        {
            if (string.IsNullOrEmpty(UserName))
            { return $"{default(DateTime)}\nGreetings, Default User!"; }
            return $"{default(DateTime)}\nGreetings, {UserName}!";
        }
    }
}
