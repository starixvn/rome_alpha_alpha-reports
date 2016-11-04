namespace Alpha.Reports.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class EnumerableExtensions
    {
        public static IEnumerable<T> OrderBy<T>(
            this IEnumerable<T> list,
            string sortPropertyName,
            bool descending = true)
        {
            if (string.IsNullOrWhiteSpace(sortPropertyName))
            {
                return list;
            }

            PropertyInfo prop = typeof(T).GetProperty(sortPropertyName);

            if (prop == null)
            {
                return list;
            }

            return (descending) ?
                  list.OrderByDescending(x => prop.GetValue(x, null))
                : list.OrderBy(x => prop.GetValue(x, null));
        }
    }
}
