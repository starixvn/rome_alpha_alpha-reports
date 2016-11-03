namespace Alpha.Reports.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    public static class IEnumerableHelper
    {
        public static IEnumerable<T> Sort<T>(IEnumerable<T> enumerable, string columnName, int direction)
        {
            var property = typeof(T).GetProperty(columnName);
            var list = enumerable.ToList();

            list.Sort((t1, t2) =>
            {
                var col1 = property.GetValue(t1);

                var col2 = property.GetValue(t2);

                return direction * Comparer<object>.Default.Compare(col1, col2);
            });

            return list;
        }
    }
}
