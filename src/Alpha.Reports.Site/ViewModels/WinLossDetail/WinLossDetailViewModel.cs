namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Components;
    using Constants;
    using Extensions;
    using Helpers;
    using Params;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using Requests.WinLossDetail;

    public class WinLossDetailViewModel
    {
        private const int AllBetProductId = 21;
        private readonly WinLossDetailQuery message;
        private readonly WinLossDetailParam winLossDetailParam;

        public WinLossDetailViewModel(
            WinLossDetailQuery message,
            WinLossDetailParam winLossDetailParam,
            ReportData<WinLossDetailModel> reportData,
            IEnumerable<CustInfo> breadcrumbLevels,
            IEnumerable<SelectListItem> products)
        {
            this.message = message;
            this.winLossDetailParam = winLossDetailParam;

            Title = GetReportNameByLevel(message.CustLevel);
            UserSelectedProductIds = string.Join(",", winLossDetailParam.UserSelectedProductIds);

            ReportData = reportData;
            var canSort = reportData.DataSource != null && reportData.DataSource.Count() > 1;
            SortPropertyName = canSort ? winLossDetailParam.SortByPropertyName : DefaultVariables.SortPropertyName;
            IsDescendingSort = canSort && winLossDetailParam.IsDescendingSort;

            BetListPath = Helpers.UrlHelper.MapExMainPath("~/_BetList/BetList.aspx");
            BreadcrumbLevels = breadcrumbLevels;
            BreadcrumbLevelsJson = JsonHelper.ToJsonFunc(breadcrumbLevels);
            Products = products;
        }

        public int CustId => winLossDetailParam.CustId;

        public string CustName => message.CustName;

        public DateTime MinDate => IsHistoricReport
            ? DateTime.Now.AddYears(-3).FirstDayOfYear()
            : DateTime.Now.AddMonths(-3).FirstDayOfMonth();

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => winLossDetailParam.FromDate;

        public DateTime ToDate => winLossDetailParam.ToDate;

        public string Language => message.Language;

        public string UserSelectedProductIds { get; }

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public string SortPropertyName { get; }

        public bool IsDescendingSort { get; }

        public bool IsStackMode => message.IsStackMode;

        public bool IsHistoricReport => message.IsHistoricReport;

        public bool IsFilterVisible => message.IsFilterVisible;

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

        public IEnumerable<SelectListItem> Products { get; }

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

        public string Title { get; }

        public bool IsSetDefaultStackMode => message.IsSetDefaultStackMode;

        public UserLevel CustLevel => message.CustLevel;

        public ReportData<WinLossDetailModel> ReportData { get; }

        public IEnumerable<CustInfo> BreadcrumbLevels { get; }

        public string BreadcrumbLevelsJson { get; }

        private string BetListPath { get; }

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "Reviewed")]
        public string BuildDrillDownLink(
            WinLossDetailModel model,
            string currentUrl)
        {
            if (CustLevel.IsAgentLevel())
            {
                return BuildBetListLink(model);
            }

            return BuildDownlineLink(model, currentUrl);
        }

        private string BuildDownlineLink(WinLossDetailModel model, string currentUrl)
        {
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

        private string BuildBetListLink(WinLossDetailModel model)
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

        private string GetReportNameByLevel(UserLevel userLevel)
            => userLevel.IsSuperLevel()
            ? Resources.MasterWinLossDetail
            : userLevel.IsMasterLevel()
                ? Resources.AgentWinLossDetail
                : Resources.MemberWinLossDetail;
    }
}