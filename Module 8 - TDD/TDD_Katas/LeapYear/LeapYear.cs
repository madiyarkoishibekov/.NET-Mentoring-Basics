using System;

namespace LeapYearKata
{
    public class LeapYear
    {
        public bool IsLeapYear(int number)
        {
            bool result = false;
            if ((number % 4 == 0) && ((number % 100 != 0) || (number % 400 == 0)) )
            {
                result = true;
            }

            return result;
        }
    }
}
