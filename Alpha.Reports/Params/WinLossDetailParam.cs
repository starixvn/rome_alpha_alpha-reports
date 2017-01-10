namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Extensions;
    using Models;
    using Services;

    public class WinLossDetailParam
    {
        public WinLossDetailParam(
            int custId,
            int pageIndex,
            int pageSize,
            DateTime fromDate,
            DateTime toDate,
            IEnumerable<int> userSelectedProductIds,
            UserLevel reportLevel,
            string sortByPropertyName,
            bool isDescendingSort,
            bool isHistoricReport)
        {
            CustId = custId;
            PageIndex = pageIndex == 0 ? DefaultVariables.PageIndex : pageIndex;
            PageSize = pageSize == 0 ? DefaultVariables.PageSize : pageSize;

            IsHistoryReport = isHistoricReport;

            if (fromDate == DateTime.MinValue)
            {
                FromDate = isHistoricReport ? DateTimeManager.Now.FirstDayOfMonth() : DateTimeManager.Now;
            }
            else
            {
                FromDate = fromDate;
            }

            ToDate = toDate == DateTime.MinValue ? DateTimeManager.Now : toDate;

            ReportLevel = reportLevel;

            SortByPropertyName = string.IsNullOrWhiteSpace(sortByPropertyName)
                ? DefaultVariables.SortPropertyName
                : GetSortedProperty(sortByPropertyName);

            UserSelectedProductIds = userSelectedProductIds;
            IsDescendingSort = isDescendingSort;
        }

        public int CustId { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public UserLevel ReportLevel { get; }

        public string SortByPropertyName { get; }

        public bool IsDescendingSort { get; }

        public bool IsHistoryReport { get; }

        private string GetSortedProperty(string sortedProperty)
        {
            var isInRange =
                ReportLevel.IsSuperLevel()
                || (ReportLevel.IsMasterLevel() && !sortedProperty.StartsWith("Super", StringComparison.OrdinalIgnoreCase))
                || (ReportLevel.IsAgentLevel()
                        && !sortedProperty.StartsWith("Super", StringComparison.OrdinalIgnoreCase)
                        && !sortedProperty.StartsWith("Master", StringComparison.OrdinalIgnoreCase));

            if (!isInRange)
            {
                return DefaultVariables.SortPropertyName;
            }

            return sortedProperty;
        }

        public bool IsValid()
        {
            if (IsHistoryReport)
            {
                return DateTimeManager.Now.AddYears(-3).FirstDayOfYear() <= FromDate
                    && FromDate <= ToDate
                    && ToDate.Year == FromDate.Year
                    && ToDate.Month == FromDate.Month;
            }

            return DateTimeManager.Now.AddMonths(-3).FirstDayOfMonth() <= FromDate
                   && FromDate <= ToDate;
        }
    }
}