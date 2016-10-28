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
            int totalRecords,
            DateTime fromDate,
            DateTime toDate)
        {
            DataSource = dataSource ?? Enumerable.Empty<T>();
            CurrentPageIndex = currentPageIndex;
            TotalRecords = totalRecords;

            FromDate = fromDate;
            ToDate = toDate;
        }

        public IEnumerable<T> DataSource { get; }

        public int CurrentPageIndex { get; }

        public int TotalRecords { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }
    }
}