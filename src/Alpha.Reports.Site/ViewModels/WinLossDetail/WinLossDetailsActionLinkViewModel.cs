namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Requests.WinLossDetail;

    public class WinLossDetailsActionLinkViewModel
    {
        public WinLossDetailsActionLinkViewModel(
            int custId,
            string custName,
            byte userLevelId,
            DateTime fromDate,
            DateTime toDate,
            int userSelectedDateRangeOption,
            string userSelectedProductIds,
            string sortPropertyName,
            bool isDescendingSort,
            bool isHictoricReport,
            bool isFilterVisible,
            bool isStackMode,
            int pageSize,
            string drillDownInfoJson)
        {
            CustId = custId;
            CustName = custName;
            UserLevelId = userLevelId;
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
            DrillDownInfoJson = drillDownInfoJson;
        }

        public int CustId { get; }

        public string CustName { get; }

        public byte UserLevelId { get; }

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

        public string DrillDownInfoJson { get; }
    }
}