namespace Alpha.Reports.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class StringExtensions
    {
        private const string ShortMonthNameAndShortYear = "MMM-yy";

        public static List<int> ToListInt(this string delimitedString)
        {
            if (string.IsNullOrWhiteSpace(delimitedString))
            {
                return Enumerable.Empty<int>().ToList();
            }

            var result = 0;

            var ids = delimitedString
                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Where(str => int.TryParse(str, out result))
                .Select(str => Convert.ToInt32(str)).ToList();

            return ids;
        }

        public static bool IsNullOrBlank(this string input) => string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input);

        public static string ToShortMonthNameAndShortYear(this string dateTimeString)
        {
            if (string.IsNullOrWhiteSpace(dateTimeString))
            {
                return string.Empty;
            }

            var splitedInts = dateTimeString
                .Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(str => Convert.ToInt32(str)).ToList();

            var dateTime = new DateTime(splitedInts[0], splitedInts[1], 1);

            return dateTime.ToString(ShortMonthNameAndShortYear);
        }
    }
}