namespace Alpha.Reports.Site.ViewModels.WinLossDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Models;
    using Models.WinlossDetails;

    public class WinLossDetailViewModel
    {
        private const int AllBetProductId = 21;

        public int CustId { get; set; }

        public DateTime MinDate => IsHistoricReport
            ? DateTime.Now.AddYears(-3).FirstDayOfMonth()
            : DateTime.Now.AddMonths(-3).FirstDayOfMonth();

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Language { get; set; }

        public string UserSelectedProductIds { get; set; }

        public int UserSelectedDateRangeOption { get; set; }

        public ReportData<WinLossDetail> ReportData { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool IsFilterVisible { get; set; }

        public bool IsDirectReportViewAction { get; set; }

        public IEnumerable<string> NetTurnoverTooltips
        {
            get
            {
                var netTurnoverTooltips = new List<string>();

                if (Products.Any(product => Convert.ToInt32(product.Value) == AllBetProductId))
                {
                    netTurnoverTooltips.Add(Resources.Tooltip_NetTurnover_21);
                }

                return netTurnoverTooltips;
            }
        }

        public IEnumerable<SelectListItem> Products { get; set; }

        public PagerViewModel PagerViewModel =>
            new PagerViewModel(ReportData.CurrentPageIndex, ReportData.PageSize, ReportData.TotalRecords);

        private MonthPickerViewModel MonthPickerViewModel =>
            new MonthPickerViewModel(MinDate, FromDate, ToDate);

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        private ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(Products, UserSelectedProductIds, Language);

        public WinLossDetailFilterViewModel FilterViewModel =>
            new WinLossDetailFilterViewModel(
                CustId,
                UserLevel,
                IsHistoricReport,
                DateRangePickerViewModel,
                MonthPickerViewModel,
                ProductMultiSelectionViewModel,
                IsDirectReportViewAction,
                IsFilterVisible,
                MinDate);

        public LinkActionViewModel LinkActionViewModel(int custId) =>
            new LinkActionViewModel(
                    custId,
                    (byte)(UserLevel.Value - 1),
                    FromDate,
                    ToDate,
                    UserSelectedDateRangeOption,
                    UserSelectedProductIds,
                    SortPropertyName,
                    IsDescendingSort,
                    IsHistoricReport,
                    IsFilterVisible,
                    IsStackMode,
                    ReportData.PageSize);

        public UserLevel UserLevel { get; set; }

        public string Title
        {
            get
            {
                if (UserLevel.IsSuperLevel())
                {
                    return Resources.MasterWinLossDetail;
                }

                if (UserLevel.IsMasterLevel())
                {
                    return Resources.AgentWinLossDetail;
                }

                return Resources.MemberWinLossDetail;
            }
        }
    }
}