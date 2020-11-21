namespace AddressBook.Service.DummyTest
{
    public class FizzBuzz
    {
        public static string GetResult(int input)
        {
            if (input % 15 == 0)
            {
                return "Fizz Buzz";
            }

            if (input % 3 == 0)
            {
                return "Fizz";
            }

            if (input % 5 == 0)
            {
                return "Buzz";
            }

            return input.ToString();
        }
    }
}
