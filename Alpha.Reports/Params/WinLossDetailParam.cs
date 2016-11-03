namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            bool isHistoricReport,
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

            SortedColumn = string.IsNullOrWhiteSpace(sortedColumn)
                ? GetDefaultSortedColumn()
                : GetSortedColumn(sortedColumn);

            SortDirection = sortDirection == 0 ? SortDirectionConstant.Default : sortDirection;

            UserSelectedProductIds = string.IsNullOrWhiteSpace(userSelectedProductIds)
                ? string.Join(",", userProducts.Select(product => product.Id))
                : userSelectedProductIds;
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

        private string GetSortedColumn(string sortedColumn)
        {
            var isInRange =
                ReportLevel.IsSuperLevel()
                || (ReportLevel.IsMasterLevel() && !sortedColumn.StartsWith("Super", StringComparison.OrdinalIgnoreCase))
                || (ReportLevel.IsAgentLevel() && !sortedColumn.StartsWith("Super", StringComparison.OrdinalIgnoreCase) && !sortedColumn.StartsWith("Master", StringComparison.OrdinalIgnoreCase));

            if (isInRange)
            {
                return sortedColumn;
            }

            return GetDefaultSortedColumn();
        }
    }
}
