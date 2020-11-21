namespace AddressBook.Service.DummyTest
{
    public class Fibonacci
    {
        public static int GetResult(int input)
        {
            if (input >= 2)
            {
                return GetResult(input - 1) + GetResult(input - 2);
            }

            if (input == 1)
            {
                return 1;
            }

            return 0;
        }
    }
}
