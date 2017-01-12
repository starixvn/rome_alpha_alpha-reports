namespace Alpha.Reports.Site.ViewModels.WinLoss
{
    using System;

    public class WinLossActionLinkViewModel
    {
        public WinLossActionLinkViewModel(
            int custId,
            string custName,
            byte custLevelId,
            DateTime fromDate,
            DateTime toDate,
            int userSelectedDateRangeOption,
            string userSelectedProductIds,
            bool isHictoricReport,
            bool isFilterVisible,
            string sortPropertyName,
            bool isDescendingSort,
            bool isStackMode,
            string drillDownInfoJson)
        {
            CustId = custId;
            CustName = custName;
            CustLevelId = custLevelId;
            FromDate = fromDate;
            ToDate = toDate;
            UserSelectedDateRangeOption = userSelectedDateRangeOption;
            UserSelectedProductIds = userSelectedProductIds;
            IsHistoricReport = isHictoricReport;
            SortPropertyName = sortPropertyName;
            IsDescendingSort = isDescendingSort;
            IsFilterVisible = isFilterVisible;
            IsStackMode = isStackMode;
            DrillDownInfoJson = drillDownInfoJson;
        }

        public int CustId { get; }

        public string CustName { get; }

        public byte CustLevelId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string UserSelectedProductIds { get; }

        public bool IsHistoricReport { get; }

        public string SortPropertyName { get; }

        public bool IsDescendingSort { get; }

        public int UserSelectedDateRangeOption { get; }

        public bool IsFilterVisible { get; }

        public bool IsStackMode { get; }

        public string DrillDownInfoJson { get; }
    }
}