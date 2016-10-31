namespace Alpha.Reports.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    public static class IEnumerableHelper
    {
        public static void SortIEnumerable<T>(IEnumerable<T> list, string columnName, int direction)
        {
            var property = typeof(T).GetProperty(columnName);

            list.ToList().Sort((t1, t2) =>
            {
                var col1 = property.GetValue(t1);

                var col2 = property.GetValue(t2);

                return direction * Comparer<object>.Default.Compare(col1, col2);
            });
        }
    }
}
