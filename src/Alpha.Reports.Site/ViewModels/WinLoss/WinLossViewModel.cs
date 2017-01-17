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
    using Reports.Models;
    using Reports.Models.Winloss;
    using Web.Helpers;

    public class WinLossViewModel
    {
        public WinLossViewModel(List<WinLossModel> reportData, UserLevel custLevel)
        {
            CustLevel = custLevel;
            BuildResources();

            if (reportData != null && reportData.Any())
            {
                ReportData = reportData.OrderBy(x => x.CustName).ToList();
                int index, count;
                for (index = 1, count = reportData.Count, GrantTotal = reportData[0]; index < count; index++)
                {
                    GrantTotal = WinLossModel.Add(GrantTotal, reportData[index]);
                }
            }
            else
            {
                ReportData = Enumerable.Empty<WinLossModel>();
                GrantTotal = new WinLossModel();
            }
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public DateTime MinDate => IsHistoricReport
            ? DateTime.Now.AddYears(-3).FirstDayOfMonth()
            : DateTime.Now.AddMonths(-3).FirstDayOfMonth();

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Language { get; set; }

        public string UserSelectedProductIds { get; set; }

        public int UserSelectedDateRangeOption { get; set; }

        public IEnumerable<WinLossModel> ReportData { get; set; }

        public WinLossModel GrantTotal { get; }

        public bool IsStackMode { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool IsFilterVisible { get; set; }

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
                CustName,
                CustLevel,
                IsHistoricReport,
                DateRangePickerViewModel,
                MonthPickerViewModel,
                ProductMultiSelectionViewModel,
                IsFilterVisible,
                MinDate,
                BreadcrumbLevelsJson);

        public UserLevel CustLevel { get; }

        public IEnumerable<CustInfo> BreadcrumbLevels { get; set; }

        public string BreadcrumbLevelsJson { get; set; }

        public string Title { get; private set; }

        public string CustLevelName { get; private set; }

        public string DownLineLevelName { get; private set; }

        public string CustLevelNetWinLossName { get; private set; }

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

        public BreadcrumbLevelsViewModel BreadcrumbLevelsViewModel =>
           new BreadcrumbLevelsViewModel(BreadcrumbLevels, BreadcrumbLevelsJson);

        public virtual string BetListPath => MicroSites.OldMainUrl("~/_BetList/BetList.aspx");

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
    }
}