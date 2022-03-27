using System;

namespace Task2
{
    public class NumberParser : INumberParser
    {
        public int Parse(string stringValue)
        {
            if (stringValue == null)
            {
                throw new ArgumentNullException(stringValue);
            }

            int intValue;

            try
            {
                intValue = Convert.ToInt32(stringValue);
                return intValue;
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}