namespace Alpha.Reports.Site.ViewModels.WinLoss
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
    using Params;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Requests.WinLoss;

    public class WinLossViewModel
    {
        private readonly string sortPropertyNameDefault = "CustName";
        private readonly WinLossQuery message;
        private readonly WinLossParam winLossParam;

        public WinLossViewModel(
            IEnumerable<WinLossModel> reportData,
            WinLossModel grandTotal,
            WinLossQuery message,
            WinLossParam winLossParam,
            IEnumerable<CustInfo> breadcrumbLevels,
            IEnumerable<SelectListItem> products)
        {
            this.message = message;
            this.winLossParam = winLossParam;

            Products = products;
            BreadcrumbLevels = breadcrumbLevels;
            ReportData = reportData;
            GrantTotal = grandTotal;

            BuildResources();
        }

        public int CustId => message.CustId;

        public string CustName => message.CustName;

        public DateTime MinDate => IsHistoricReport
            ? DateTime.Now.AddYears(-3).FirstDayOfMonth()
            : DateTime.Now.AddMonths(-3).FirstDayOfMonth();

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => winLossParam.FromDate;

        public DateTime ToDate => winLossParam.ToDate;

        public string Language => message.Language;

        public string UserSelectedProductIds => string.Join(",", winLossParam.UserSelectedProductIds);

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public IEnumerable<WinLossModel> ReportData { get; }

        public WinLossModel GrantTotal { get; }

        public bool IsStackMode => message.IsStackMode;

        public string SortPropertyName => GetSortedProperty(message.CustLevel, message.SortPropertyName);

        public bool IsDescendingSort => message.IsDescendingSort;

        public bool IsHistoricReport => message.IsHistoricReport;

        public bool IsFilterVisible => message.IsFilterVisible;

        public IEnumerable<SelectListItem> Products { get; }

        public UserLevel CustLevel => message.CustLevel;

        public IEnumerable<CustInfo> BreadcrumbLevels { get; }

        public string BreadcrumbLevelsJson => JsonHelper.ToJsonFunc(BreadcrumbLevels);

        public string Title { get; private set; }

        public string CustLevelName { get; private set; }

        public string DownLineLevelName { get; private set; }

        public string CustLevelNetWinLossName { get; private set; }

        private MonthPickerViewModel MonthPickerViewModel =>
            new MonthPickerViewModel(MinDate, FromDate, ToDate);

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        private ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(Products, UserSelectedProductIds, Language);

        public WinLossFilterViewModel FilterViewModel =>
            new WinLossFilterViewModel(
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

        public string BetListPath => Helpers.UrlHelper.MapExMainPath("~/_BetList/BetList.aspx");

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "Reviewed")]
        public string BuildDrillDownLink(
            WinLossModel model,
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

        private string GetSortedProperty(UserLevel custLevel, string sortedProperty)
        {
            if (string.IsNullOrEmpty(sortedProperty))
            {
                return sortPropertyNameDefault;
            }

            var isSortedPropertyInRange =
                custLevel.IsSuperLevel()
                || (custLevel.IsMasterLevel() && !sortedProperty.StartsWith("Super", StringComparison.OrdinalIgnoreCase))
                || (custLevel.IsAgentLevel()
                        && !sortedProperty.StartsWith("Super", StringComparison.OrdinalIgnoreCase)
                        && !sortedProperty.StartsWith("Master", StringComparison.OrdinalIgnoreCase));

            return isSortedPropertyInRange ? sortedProperty : sortPropertyNameDefault;
        }

        private void BuildResources()
        {
            if (CustLevel.IsSuperLevel())
            {
                this.Title = Resources.MasterWinLoss;
                this.CustLevelName = Resources.Super;
                this.DownLineLevelName = Resources.Master;
                this.CustLevelNetWinLossName = "SuperNetWinLoss";
            }
            else if (CustLevel.IsMasterLevel())
            {
                this.Title = Resources.AgentWinLoss;
                this.CustLevelName = Resources.Master;
                this.DownLineLevelName = Resources.Agent;
                this.CustLevelNetWinLossName = "MasterNetWinLoss";
            }
            else
            {
                this.Title = Resources.MemberWinLoss;
                this.CustLevelName = Resources.Agent;
                this.DownLineLevelName = Resources.Member;
                this.CustLevelNetWinLossName = "AgentNetWinLoss";
            }
        }
    }
}