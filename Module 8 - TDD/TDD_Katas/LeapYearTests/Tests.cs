//## Leap Year Kata

//Write a function that returns true or false depending on
//whether its input integer is a leap year or not.

//A leap year is defined as one that is divisible by 4,
//but is not otherwise divisible by 100 unless it is
//also divisble by 400.

//For example, 2001 is a typical common year and 1996
//is a typical leap year, whereas 1900 is an atypical
//common year and 2000 is an atypical leap year.


using NUnit.Framework;

namespace LeapYearKata
{
    public class Tests
    {
        [Test]
        public void LeapYear_NonDivisibleBy4_ReturnsFalse()
        {
            //arrange
            var yearChecker = new LeapYear();

            //act
            var actual = yearChecker.IsLeapYear(5);

            //assert
            Assert.IsFalse(actual);
        }

        [Test]
        public void LeapYear_DivisibleBy4AndNonDivisibleBy100AndNonDivisibleBy400_ReturnsTrue()
        {
            //arrange
            var yearChecker = new LeapYear();

            //act
            var actual = yearChecker.IsLeapYear(1996);

            //assert
            Assert.IsTrue(actual);
        }

        [Test]
        [TestCase(2001, false)]
        [TestCase(1996, true)]
        [TestCase(1900, false)]
        [TestCase(2000, true)]
        public void LeapYear_VariousLeapAndCommonYears_ReturnsRespectively(int number, bool expected)
        {
            //arrange
            var yearChecker = new LeapYear();

            //act
            var actual = yearChecker.IsLeapYear(number);

            //assert
            Assert.That (actual == expected);
        }
    }
}