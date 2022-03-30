using System;
using System.Collections.Generic;

namespace Task2
{
    public class NumberParser : INumberParser
    {
        public int Parse(string stringValue)
        {
            if (stringValue == null)
            {
                throw new ArgumentNullException("No string provided.");
            }

            stringValue = stringValue.Trim();
            var digits = new List<char>();
            var isNegativeNumber = false;
            digits.AddRange(stringValue.ToCharArray());
            if (digits.Count > 0 && (digits[0] == '-' || digits[0] == '+'))
            {
                isNegativeNumber = digits[0] == '-';
                digits.RemoveAt(0);
            }

            if (!digits.TrueForAll(c => (c >= '0') && (c <= '9')) || digits.Count == 0)
            {
                throw new FormatException("Not a number.");
            }

            var result = GetIntFromListOfDigits(digits, isNegativeNumber);
            return result;
        }

        private int GetIntFromListOfDigits(List<char> digits, bool isNegativeNumber)
        {
            var multiplier = isNegativeNumber ? -1 : 1;
            int result = 0;
            for (int i = digits.Count - 1; i >= 0; i--)
            {
                result = checked(result + (digits[i] - '0') * multiplier);
                multiplier *= 10;
            }

            return result;
        }
    }
}