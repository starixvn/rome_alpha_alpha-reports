namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Components;
    using Extensions;
    using Models;
    using Models.WinlossDetail;
    using Resources;

    public class WinLossDetailViewModel<T> where T : MemberWinLossDetail
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

        public ReportData<T> ReportData { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool HideFilter { get; set; }

        public IEnumerable<string> NetTurnoverTooltips
        {
            get
            {
                var netTurnoverTooltips = new List<string>();

                if (Products.Any(product => Convert.ToInt32(product.Value) == AllBetProductId))
                {
                    netTurnoverTooltips.Add(SiteResources.Tooltip_NetTurnover_21);
                }

                return netTurnoverTooltips;
            }
        }

        public IEnumerable<SelectListItem> Products { get; set; }

        public bool IsDirectReportViewAction { get; set; }

        public PagerViewModel Pager =>
            new PagerViewModel(ReportData.CurrentPageIndex, ReportData.PageSize, ReportData.TotalRecords);

        public MonthPickerViewModel MonthPicker =>
            new MonthPickerViewModel(MinDate, FromDate, ToDate);

        public DateRangePickerViewModel DateRangePicker => 
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        public ProductMultiSelectionViewModel ProductMultiSelection =>
            new ProductMultiSelectionViewModel(Products, Language);

        public string ViewDataWarningMessage => 
            string.Format(IsHistoricReport
               ? SiteResources.Warning_CanOnlyViewDataAfterTime_BackToCurrentReport
               : SiteResources.Warning_CanOnlyViewDataAfterTime_ClickToViewHistoricReport,
            MinDate);
    }
}