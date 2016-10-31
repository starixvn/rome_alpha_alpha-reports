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
            T total)
        {
            DataSource = dataSource ?? Enumerable.Empty<T>();
            CurrentPageIndex = currentPageIndex;
            PageSize = pageSize;
            TotalRecords = totalRecords;

            Total = total;
        }

        public IEnumerable<T> DataSource { get; }

        public T Total { get; }

        public int CurrentPageIndex { get; }

        public int PageSize { get; }

        public int TotalRecords { get; }

        public int NumberRecordsOfCurrentPage
        {
            get
            {
                return DataSource != null ? DataSource.Count() : 0;
            }
        }
    }
}