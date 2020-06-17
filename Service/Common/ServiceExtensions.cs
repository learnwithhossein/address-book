using System;

namespace Service.Common
{
    public static class ServiceExtensions
    {
        public static double Power(this int number, int power)
        {
            return Math.Pow(number, power);
        }
    }
}
