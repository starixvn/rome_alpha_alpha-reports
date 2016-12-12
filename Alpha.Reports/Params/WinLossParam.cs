namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Extensions;
    using Models;

    public class WinLossParam
    {
        private ApplicationContext applicationContext;

        public WinLossParam(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            IEnumerable<int> userSelectedProductIds,
            UserLevel reportLevel,
            string sortByPropertyName,
            bool isDescendingSort,
            bool isHistoricReport,
            ApplicationContext applicationContext = null)
        {
            this.applicationContext = applicationContext ?? new ReportApplicationContext();

            CustId = custId;

            IsHistoryReport = isHistoricReport;

            if (fromDate == DateTime.MinValue)
            {
                FromDate = isHistoricReport ? DateTime.Now.FirstDayOfMonth() : DateTime.Now;
            }
            else
            {
                FromDate = fromDate;
            }

            ToDate = toDate == DateTime.MinValue ? DateTime.Now : toDate;

            ReportLevel = reportLevel;

            SortByPropertyName = string.IsNullOrWhiteSpace(sortByPropertyName)
                ? GetDefaultSortedProperty()
                : GetSortedProperty(sortByPropertyName);

            UserSelectedProductIds = userSelectedProductIds;
            IsDescendingSort = isDescendingSort;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public UserLevel ReportLevel { get; }

        public string SortByPropertyName { get; }

        public bool IsHistoryReport { get; }

        public bool IsDescendingSort { get; }

        private string GetDefaultSortedProperty()
        {
            if (ReportLevel.IsSuperLevel())
            {
                return "SuperNetWinLoss.WinLoss";
            }

            if (ReportLevel.IsMasterLevel())
            {
                return "MasterNetWinLoss.WinLoss";
            }

            return "AgentNetWinLoss.WinLoss";
        }

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
                return GetDefaultSortedProperty();
            }

            return sortedProperty;
        }

        public bool IsValid()
        {
            if (IsHistoryReport)
            {
                return applicationContext.CurrentDate.AddYears(-3).FirstDayOfYear() <= FromDate
                    && FromDate <= ToDate
                    && ToDate.Year == FromDate.Year
                    && ToDate.Month == FromDate.Month;
            }

            return applicationContext.CurrentDate.AddMonths(-3).FirstDayOfMonth() <= FromDate
                   && FromDate <= ToDate;
        }
    }
}
