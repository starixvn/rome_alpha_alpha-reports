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

    public class WinLossViewModel : HasDownlineLinkReportViewModel<WinLossModel>
    {
        private readonly string sortPropertyNameDefault = "CustName";

        public WinLossViewModel(
            IEnumerable<WinLossModel> reportData,
            WinLossQuery message,
            WinLossParam winLossParam,
            IEnumerable<CustInfo> breadcrumbLevels,
            IEnumerable<SelectListItem> products) : base(message.CustLevel, breadcrumbLevels)
        {
            CustId = message.CustId;
            CustName = message.CustName;
            FromDate = winLossParam.FromDate;
            ToDate = winLossParam.ToDate;
            Language = message.Language;

            Products = products;
            IsStackMode = message.IsStackMode;
            IsHistoricReport = message.IsHistoricReport;
            IsFilterVisible = message.IsFilterVisible;
            IsDescendingSort = message.IsDescendingSort;
            SortPropertyName = GetSortedProperty(message.CustLevel, message.SortPropertyName);
            UserSelectedDateRangeOption = message.UserSelectedDateRangeOption;
            UserSelectedProductIds = string.Join(",", winLossParam.UserSelectedProductIds);

            ReportData = reportData;

            AccumulateGrantTotal(reportData);
            BuildResources();
        }

        public int CustId { get; }

        public string CustName { get; }

        public DateTime MinDate => IsHistoricReport
            ? DateTime.Now.AddYears(-3).FirstDayOfMonth()
            : DateTime.Now.AddMonths(-3).FirstDayOfMonth();

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string Language { get; }

        public string UserSelectedProductIds { get; }

        public int UserSelectedDateRangeOption { get; }

        public IEnumerable<WinLossModel> ReportData { get; private set; }

        public WinLossModel GrantTotal { get; private set; }

        public bool IsStackMode { get; }

        public string SortPropertyName { get; }

        public bool IsDescendingSort { get; }

        public bool IsHistoricReport { get; }

        public bool IsFilterVisible { get; }

        public IEnumerable<SelectListItem> Products { get; }

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

        public string Title { get; private set; }

        public string CustLevelName { get; private set; }

        public string DownLineLevelName { get; private set; }

        public string CustLevelNetWinLossName { get; private set; }

        public BreadcrumbLevelsViewModel BreadcrumbLevelsViewModel =>
           new BreadcrumbLevelsViewModel(BreadcrumbLevels, BreadcrumbLevelsJson);

        private void AccumulateGrantTotal(IEnumerable<WinLossModel> reportData)
        {
            var reportDataCopy = reportData.ToList();
            if (reportDataCopy != null && reportDataCopy.Any())
            {
                int index, count;
                for (index = 1, count = reportDataCopy.Count, GrantTotal = reportDataCopy[0]; index < count; index++)
                {
                    GrantTotal = WinLossModel.Add(GrantTotal, reportDataCopy[index]);
                }
            }
            else
            {
                ReportData = Enumerable.Empty<WinLossModel>();
                GrantTotal = new WinLossModel();
            }
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

        protected override IDictionary<string, object> GetDownlineLinkRouteValues(WinLossModel model)
        {
            return new Dictionary<string, object>
            {
                { "CustId", model.CustId },
                { "CustName", model.CustName }
            };
        }

        protected override IDictionary<string, object> GetBetListLinkRouteValues(WinLossModel model)
        {
            return new Dictionary<string, object>
            {
                { "Custid", model.CustId },
                { "Username", model.CustName },
                { "FDate",  FromDate },
                { "TDate", ToDate },
                { "ProductIds", UserSelectedProductIds },
                { "FilterPostback", 1 },
                { "Type", "WLByDate" }
            };
        }
    }
}