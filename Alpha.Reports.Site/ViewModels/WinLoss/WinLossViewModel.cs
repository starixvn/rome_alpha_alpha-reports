namespace Alpha.Reports.Site.ViewModels.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Models;
    using Models.Winloss;
    using Requests.WinLoss;
    using WinLossDetails;

    public class WinLossViewModel
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

        public ReportData<WinLoss> ReportData { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool IsFilterVisible { get; set; }

        public bool IsDirectReportViewAction { get; set; }

        public IEnumerable<SelectListItem> Products { get; set; }

        private MonthPickerViewModel MonthPickerViewModel =>
            new MonthPickerViewModel(MinDate, FromDate, ToDate);

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        private ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(Products, UserSelectedProductIds, Language);

        public WinLossFilterViewModel FilterViewModel =>
            new WinLossFilterViewModel(
                CustId,
                UserLevel,
                IsHistoricReport,
                DateRangePickerViewModel,
                MonthPickerViewModel,
                ProductMultiSelectionViewModel,
                IsDirectReportViewAction,
                IsFilterVisible,
                MinDate,
                ((WinLossQuery)BreadcrumbLevels.Last().RouteValues).BreadcrumbLevels);

        public WinLossActionLinkViewModel LinkActionViewModel(WinLoss winLossDetail)
        {
            if (winLossDetail == null)
            {
                throw new ArgumentNullException("winLoss");
            }

            return new WinLossActionLinkViewModel(
                    winLossDetail.CustId,
                    winLossDetail.CustName,
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
                    ReportData.PageSize,
                    BreadcrumbLevels);
        }

        public UserLevel UserLevel { get; set; }

        public string Title
        {
            get
            {
                if (UserLevel.IsSuperLevel())
                {
                    return Resources.MasterWinLoss;
                }

                if (UserLevel.IsMasterLevel())
                {
                    return Resources.AgentWinLoss;
                }

                return Resources.MemberWinLoss;
            }
        }

        public bool HasSubmittedParams { get; set; }

        public IEnumerable<ActionLinkViewModel> BreadcrumbLevels { get; set; }
    }
}