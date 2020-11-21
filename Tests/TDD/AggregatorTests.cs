using AddressBook.Service.DummyTest;
using NUnit.Framework;

namespace AddressBook.Tests.TDD
{
    [TestFixture]
    public class AggregatorTests
    {
        [Test]
        public void Given_1_and_2_as_input_should_result_3()
        {
             Assert.AreEqual(3, Aggregator.Sum(1,2));
        }

        [Test]
        public void Given_2_and_3_as_input_should_result_5()
        {
            Assert.AreEqual(5, Aggregator.Sum(2, 3));
        }
    }
}