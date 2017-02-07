namespace Alpha.Reports.Models
{
    using System.Collections.Generic;

    public class PagedReportData<T> : ReportData<T> where T : class
    {
        public PagedReportData(
            IEnumerable<T> dataSource,
            int currentPageIndex,
            int pageSize,
            int totalRecords,
            T totalData) : base(dataSource, totalData)
        {
            CurrentPageIndex = currentPageIndex;
            PageSize = pageSize;
            TotalRecords = totalRecords;
        }

        public int CurrentPageIndex { get; }

        public int PageSize { get; }

        public int TotalRecords { get; }

        public override bool Any() => TotalRecords > 0;

        public override bool MoreThanOne() => TotalRecords > 1;
    }
}