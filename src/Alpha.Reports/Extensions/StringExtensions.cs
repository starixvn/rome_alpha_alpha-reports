namespace Alpha.Reports.Extensions
{
    using Alpha.Reports.Constants;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class StringExtensions
    {
        private const string ShortMonthNameAndShortYear = "MMM-yy";
        private const string ShortMonthNameAndYear = "MMM-yyyy";

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

        public static IEnumerable<string> ToListString(this string delimitedString)
        {
            if (string.IsNullOrWhiteSpace(delimitedString))
            {
                return Enumerable.Empty<string>();
            }

            var ids = delimitedString
                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(str => str);

            return ids;
        }

        public static bool IsNullOrBlank(this string input) => string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input);

        public static string FormatNumber(this decimal numberValue)
        {
            var result = numberValue.ToString("#,##0.00");

            return numberValue < 0 && !result.StartsWith("-", StringComparison.OrdinalIgnoreCase)
                ? $"-{result}"
                : result;
        }

        public static string FormatNumber(this decimal? numberValue)
            => numberValue.HasValue ? FormatNumber(numberValue.Value) : DefaultVariables.NA;

        public static string ToShortMonthNameAndShortYear(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var dateTime = value.ToDateTime();

            return dateTime.ToString(ShortMonthNameAndShortYear);
        }

        public static string ToShortMonthNameAndYear(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var dateTime = value.ToDateTime();

            return dateTime.ToString(ShortMonthNameAndYear);
        }

        private static DateTime ToDateTime(this string value)
        {
            var splitInts = value
                .Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(str => Convert.ToInt32(str)).ToList();

            var dateTime = new DateTime(splitInts[0], splitInts[1], 1);
            return dateTime;
        }

        public static IDictionary<int, bool> ToIntDictionary(this string delimitedString)
        {
            IDictionary<int, bool> results = new Dictionary<int, bool>();

            if (!string.IsNullOrWhiteSpace(delimitedString))
            {
                delimitedString
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList()
                    .ForEach(text =>
                    {
                        if (int.TryParse(text, out var number))
                        {
                            results[number] = true;
                        }
                    });
            }

            return results;
        }
    }
}