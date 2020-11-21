using AddressBook.Service.DummyTest;
using System.Collections.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AddressBook.Tests.BDD
{
    [Binding]
    public class FizzBuzzSteps
    {
        private List<int> _inputs;
        private List<string> _outputs;

        [Given(@"input values as follows")]
        public void GivenInputValuesAsFollows(Table table)
        {
            _inputs = new List<int>();

            for (var i = 0; i < table.RowCount; i++)
            {
                var row = table.Rows[i];
                _inputs.Add(int.Parse(row["Input"]));
            }
        }

        [When(@"Sending input values to FizzBuzz application")]
        public void WhenSendingInputValuesToFizzBuzzApplication()
        {
            _outputs = new List<string>();

            foreach (var t in _inputs)
            {
                var result = FizzBuzz.GetResult(t);
                _outputs.Add(result);
            }
        }

        [Then(@"the result should be as follows")]
        public void ThenTheResultShouldBeAsFollows(Table table)
        {
            for (int i = 0; i < table.RowCount; i++)
            {
                var row = table.Rows[i];

                Assert.AreEqual(row["Result"], _outputs[i]);
            }
        }
    }
}
