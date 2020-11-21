using AddressBook.Service.DummyTest;
using NUnit.Framework;

namespace AddressBook.Tests.TDD
{
    [TestFixture]
    public class FibonacciTest
    {
        [TestCase(11, 89)]
        [TestCase(12, 144)]
        [TestCase(0, 0)]
        [TestCase(2, 1)]
        public void Given_any_number_should_result_correct_value(int input, int result)
        {
            Assert.AreEqual(result, Fibonacci.GetResult(input));
        }
    }
}
