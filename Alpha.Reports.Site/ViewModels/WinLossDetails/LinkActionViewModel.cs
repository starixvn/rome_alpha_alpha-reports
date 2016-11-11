namespace Alpha.Reports.Site.ViewModels.WinLossDetails
{
    using System;

    public class LinkActionViewModel
    {
        public LinkActionViewModel(
            int custId,
            byte reportLevelId,
            DateTime fromDate,
            DateTime toDate,
            int userSelectedDateRangeOption,
            string userSelectedProductIds,
            string sortPropertyName,
            bool isDescendingSort,
            bool isHictoricReport,
            bool isFilterVisible,
            bool isStackMode,
            int pageSize)
        {
            CustId = custId;
            ReportLevelId = reportLevelId;
            FromDate = fromDate;
            ToDate = toDate;
            UserSelectedDateRangeOption = userSelectedDateRangeOption;
            UserSelectedProductIds = userSelectedProductIds;
            SortPropertyName = sortPropertyName;
            IsDescendingSort = isDescendingSort;
            IsHistoricReport = isHictoricReport;
            IsFilterVisible = isFilterVisible;
            PageSize = pageSize;
            IsStackMode = isStackMode;
        }

        public int CustId { get; }

        public byte ReportLevelId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string UserSelectedProductIds { get; }

        public string SortPropertyName { get; }

        public bool IsDescendingSort { get; }

        public bool IsHistoricReport { get; }

        public int UserSelectedDateRangeOption { get; }

        public bool IsFilterVisible { get; }

        public bool IsStackMode { get; }

        public int PageSize { get; }
    }
}