namespace Alpha.Reports.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class ReportData<T> where T : class
    {
        public ReportData(IEnumerable<T> dataSource, int currentPageIndex, int totalRecords)
        {
            DataSource = dataSource ?? Enumerable.Empty<T>();
            CurrentPageIndex = currentPageIndex;
            TotalRecords = totalRecords;
        }

        public IEnumerable<T> DataSource { get; }

        public int CurrentPageIndex { get; }

        public int TotalRecords { get; }
    }
}
