namespace Alpha.Reports.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class ReportData<T> where T : class
    {
        public ReportData(
            IEnumerable<T> dataSource,
            int currentPageIndex,
            int pageSize,
            int totalRecords,
            T totalData)
        {
            DataSource = dataSource ?? Enumerable.Empty<T>();
            CurrentPageIndex = currentPageIndex;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalData = totalData;
        }

        public IEnumerable<T> DataSource { get; }

        public T TotalData { get; }

        public int CurrentPageIndex { get; }

        public int PageSize { get; }

        public int TotalRecords { get; }

        public bool Any() => TotalRecords > 0;

        public bool MoreThanOne() => TotalRecords > 1;
    }
}