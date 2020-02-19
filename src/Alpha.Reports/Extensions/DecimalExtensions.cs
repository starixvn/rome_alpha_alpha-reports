namespace Alpha.Reports.Extensions
{
    using System;

    public static class DecimalExtensions
    {
        private const int DefaultDecimalPlaces = 2;

        public static decimal RoundDown(this decimal value)
        {
            return RoundDown(value, DefaultDecimalPlaces);
        }

        public static decimal RoundDown(this decimal value, int decimalPlaces)
        {
            var power = Convert.ToDecimal(Math.Pow(10, decimalPlaces));
            return Math.Floor(value * power) / power;
        }
    }
}
