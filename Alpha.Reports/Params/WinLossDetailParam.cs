namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Extensions;
    using Models;

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
            bool isHistoricReport,
            bool isStackMode,
            IEnumerable<Product> userProducts)
        {
            CustId = custId;
            PageIndex = pageIndex == 0 ? DefaultVariables.PageIndex : pageIndex;
            PageSize = pageSize == 0 ? DefaultVariables.PageSize : pageSize;

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
                ? GetDefaultSortedColumn()
                : GetSortedColumn(sortByPropertyName, isStackMode);

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

        private string GetDefaultSortedColumn()
        {
            if (ReportLevel.IsSuperLevel())
            {
                return "SuperWinLoss";
            }

            if (ReportLevel.IsMasterLevel())
            {
                return "MasterWinLoss";
            }

            return "AgentWinLoss";
        }

        private string GetSortedColumn(string sortedColumn, bool isStackMode)
        {
            var isInRange =
                ReportLevel.IsSuperLevel()
                || (ReportLevel.IsMasterLevel() && !sortedColumn.StartsWith("Super", StringComparison.OrdinalIgnoreCase))
                || (ReportLevel.IsAgentLevel()
                        && !sortedColumn.StartsWith("Super", StringComparison.OrdinalIgnoreCase)
                        && !sortedColumn.StartsWith("Master", StringComparison.OrdinalIgnoreCase));

            if (!isInRange)
            {
                return GetDefaultSortedColumn();
            }

            if (!isStackMode)
            {
                return sortedColumn;
            }

            var combinedColumns = new Dictionary<string, string>()
            {
                ["netturnover"] = "Turnover",
                ["grosscomm"] = "Turnover",
                ["membercomm"] = "MemberWinLoss",
                ["membertotal"] = "MemberWinLoss",
                ["agentcomm"] = "AgentWinLoss",
                ["agenttotal"] = "AgentWinLoss",
                ["mastercomm"] = "MasterWinLoss",
                ["mastertotal"] = "MasterWinLoss",
                ["supercomm"] = "SuperWinLoss",
                ["supertotal"] = "SuperWinLoss"
            };

            return combinedColumns.ContainsKey(sortedColumn.ToLower())
                ? combinedColumns[sortedColumn.ToLower()]
                : sortedColumn;
        }
    }
}
