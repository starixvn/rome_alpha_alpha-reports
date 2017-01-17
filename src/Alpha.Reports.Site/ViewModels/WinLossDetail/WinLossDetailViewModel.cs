namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Helpers;
    using Reports.Models;
    using Reports.Models.WinLossDetail;

    public class WinLossDetailViewModel
    {
        private const int AllBetProductId = 21;

        public WinLossDetailViewModel(IEnumerable<CustInfo> breadcrumbLevels)
        {
            BetListPath = Helpers.UrlHelper.MapExMainPath("~/_BetList/BetList.aspx");
            BreadcrumbLevels = breadcrumbLevels;
            BreadcrumbLevelsJson = JsonHelper.ToJsonFunc(breadcrumbLevels);
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public DateTime MinDate => IsHistoricReport
            ? DateTime.Now.AddYears(-3).FirstDayOfYear()
            : DateTime.Now.AddMonths(-3).FirstDayOfMonth();

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Language { get; set; }

        public string UserSelectedProductIds { get; set; }

        public int UserSelectedDateRangeOption { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool IsFilterVisible { get; set; }

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
                CustName,
                CustLevel,
                IsHistoricReport,
                DateRangePickerViewModel,
                MonthPickerViewModel,
                ProductMultiSelectionViewModel,
                IsFilterVisible,
                MinDate,
                BreadcrumbLevelsJson);

        public BreadcrumbLevelsViewModel BreadcrumbLevelsViewModel =>
            new BreadcrumbLevelsViewModel(BreadcrumbLevels, BreadcrumbLevelsJson);

        public string Title { get; set; }

        public bool IsSetDefaultStackMode { get; set; }

        public UserLevel CustLevel { get; set; }

        public ReportData<WinLossDetailModel> ReportData { get; set; }

        public IEnumerable<CustInfo> BreadcrumbLevels { get; }

        public string BreadcrumbLevelsJson { get; }

        public string BetListPath { get; }

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "Reviewed")]
        public string BuildDrillDownLink(
            WinLossDetailModel model,
            string currentUrl)
        {
            if (CustLevel.IsAgentLevel())
            {
                return Helpers.UrlHelper.AddQueryStringParamsFunc(
                    BetListPath,
                    new Dictionary<string, object>
                     {
                         { "Custid", model.CustId },
                         { "Username",  model.CustName },
                         { "FDate",  FromDate },
                         { "TDate",  ToDate },
                         { "ProductIds",  UserSelectedProductIds },
                         { "FilterPostback",  1 },
                         { "Type",  "WLByDate" }
                     });
            }

            return Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc(
                currentUrl,
                new Dictionary<string, object>
                {
                    { "CustId", model.CustId },
                    { "CustName", model.CustName },
                    { "CustLevelId", CustLevel.GetDirectDownLineLevel().Value },
                    { "BreadcrumbLevelsJson", BreadcrumbLevelsJson }
                });
        }
    }
}