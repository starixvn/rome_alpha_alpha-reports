namespace Alpha.Reports.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ReportData<T> where T : class
    {
        public ReportData(
            IEnumerable<T> dataSource,
            int currentPageIndex,
            int pageSize,
            int totalRecords,
            DateTime fromDate,
            DateTime toDate,
            T total)
        {
            DataSource = dataSource ?? Enumerable.Empty<T>();
            CurrentPageIndex = currentPageIndex;
            PageSize = pageSize;
            TotalRecords = totalRecords;

            FromDate = fromDate;
            ToDate = toDate;

            Total = total;
        }

        public IEnumerable<T> DataSource { get; }

        public T Total { get; }

        public int CurrentPageIndex { get; }

        public int PageSize { get; }

        public int TotalRecords { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int NumberRecordsOfCurrentPage
        {
            get
            {
                return DataSource != null ? DataSource.Count() : 0;
            }
        }
    }
}