namespace Alpha.Reports.Params
{
    using System;
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
            string userSelectedProductIds,
            UserLevel reportLevel,
            string sortedColumn,
            int sortDirection,
            bool isHistoricReport)
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

            SortedColumn = string.IsNullOrWhiteSpace(sortedColumn)
                ? GetDefaultSortedColumn()
                : GetSortedColumn(sortedColumn);

            SortDirection = sortDirection == 0 ? SortDirectionConstant.Default : sortDirection;
        }

        public int CustId { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string UserSelectedProductIds { get; }

        public UserLevel ReportLevel { get; }

        public string SortedColumn { get; }

        public int SortDirection { get; }

        private string GetDefaultSortedColumn()
        {
            if (ViewerLevel.IsSuperLevel())
            {
                return "SuperWinLoss";
            }

            if (ViewerLevel.IsMasterLevel())
            {
                return "MasterWinLoss";
            }

            return "AgentWinLoss";
        }

        private string GetSortedColumn(string sortedColumn)
        {
            var isInRange =
                ViewerLevel.IsSuperLevel()
                || (ViewerLevel.IsMasterLevel() && !sortedColumn.StartsWith("Super", StringComparison.OrdinalIgnoreCase))
                || (ViewerLevel.IsAgentLevel() && !sortedColumn.StartsWith("Super", StringComparison.OrdinalIgnoreCase) && !sortedColumn.StartsWith("Master", StringComparison.OrdinalIgnoreCase));

            if (isInRange)
            {
                return sortedColumn;
            }

            return GetDefaultSortedColumn();
        }
    }
}
