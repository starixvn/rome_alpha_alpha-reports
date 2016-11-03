namespace Alpha.Reports.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class StringExtensions
    {
        public static List<int> ToListInt(this string delimitedString)
        {
            if (string.IsNullOrWhiteSpace(delimitedString))
            {
                return Enumerable.Empty<int>().ToList();
            }

            var result = 0;

            var ids = delimitedString
                .Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Where(str => int.TryParse(str, out result))
                .Select(str => Convert.ToInt32(str)).ToList();

            return ids;
        }
    }
}
