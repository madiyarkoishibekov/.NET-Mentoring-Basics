using System;

namespace StringSum
{
    public class StringSum
    {
        public string Sum(string num1, string num2)
        {
            if (string.IsNullOrEmpty(num1) || string.IsNullOrEmpty(num2))
            {
                throw new ArgumentException();
            }

            if (!uint.TryParse(num1, out uint number1))
            {
                number1 = 0;
            }

            if (!uint.TryParse(num2, out uint number2))
            {
                number2 = 0;
            }

            var result = (number1 + number2).ToString();
            return result;
        }
    }
}
