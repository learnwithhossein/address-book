using AddressBook.Service.DummyTest;
using NUnit.Framework;

namespace AddressBook.Tests.TDD
{
    [TestFixture]
    public class FizzBuzzTests
    {
        [Test]
        public void Given_1_should_result_1()
        {
            Assert.AreEqual("1", FizzBuzz.GetResult(1));
        }

        [Test]
        public void Given_2_should_result_2()
        {
            Assert.AreEqual("2", FizzBuzz.GetResult(2));
        }

        [Test]
        public void Given_3_should_result_Fizz()
        {
            Assert.AreEqual("Fizz", FizzBuzz.GetResult(3));
        }

        [Test]
        public void Given_4_should_result_4()
        {
            Assert.AreEqual("4", FizzBuzz.GetResult(4));
        }

        [Test]
        public void Given_5_should_result_Buzz()
        {
            Assert.AreEqual("Buzz", FizzBuzz.GetResult(5));
        }

        [TestCase( 1, "1")]
        [TestCase( 2, "2")]
        [TestCase( 3, "Fizz")]
        [TestCase(4, "4")]
        [TestCase( 5, "Buzz")]
        [TestCase( 6, "Fizz")]
        [TestCase( 7, "7")]
        [TestCase( 8, "8")]
        [TestCase( 9, "Fizz")]
        [TestCase( 10, "Buzz")]
        [TestCase( 11, "11")]
        [TestCase( 12, "Fizz")]
        [TestCase( 13, "13")]
        [TestCase( 14, "14")]
        [TestCase( 15, "Fizz Buzz")]
        [TestCase( 16, "16")]
        public void Given_5_should_result_Buzz(int input,string result)
        {
            Assert.AreEqual(result, FizzBuzz.GetResult(input));
        }
    }
}
