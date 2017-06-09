namespace Alpha.Reports.Site.ViewModels.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Params;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Requests.WinLoss;

    public class WinLossViewModel : DrillDownableReportViewModel<WinLossModel>
    {
        private readonly string sortPropertyNameDefault = "CustName";
        private readonly WinLossQuery message;
        private readonly WinLossParam winLossParam;
        public static readonly string NetWinLossWinLoss = $"{nameof(WinLossModel.AgentNetWinLoss)}.{nameof(NetWinLoss.WinLoss)}";
        public static readonly string NetWinLossComm = $"{nameof(WinLossModel.AgentNetWinLoss)}.{nameof(NetWinLoss.Comm)}";
        public static readonly string NetWinLossTotal = $"{nameof(WinLossModel.AgentNetWinLoss)}.{nameof(NetWinLoss.Total)}";

        public WinLossViewModel(
            IEnumerable<WinLossModel> reportData,
            WinLossModel grandTotal,
            WinLossQuery message,
            WinLossParam winLossParam,
            IEnumerable<CustInfo> breadcrumbLevel,
            IEnumerable<SelectListItem> products) : base(message.CustLevel, breadcrumbLevel)
        {
            this.message = message;
            this.winLossParam = winLossParam;

            Products = products;
            ReportData = reportData;
            GrandTotal = grandTotal;

            BuildResources();
        }

        public int CustId => message.CustId;

        public string CustName => message.CustName;

        public DateTime MinDate => message.MinDate;

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => winLossParam.FromDate;

        public DateTime ToDate => winLossParam.ToDate;

        public string Language => message.Language;

        public string UserSelectedProductIds => winLossParam.UserSelectedProductIds.JoinWithComma();

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public IEnumerable<WinLossModel> ReportData { get; }

        public WinLossModel GrandTotal { get; }

        public bool IsSetDefaultStackMode => message.IsSetDefaultStackMode;

        public bool IsStackMode => message.IsStackMode;

        public string SortPropertyName => GetSortedProperty(message.CustLevel, message.SortPropertyName);

        public bool IsDescendingSort => message.IsDescendingSort;

        public bool IsHistoricReport => message.IsHistoricReport;

        public bool IsFilterVisible => message.IsFilterVisible;

        public IEnumerable<SelectListItem> Products { get; }

        public string Title { get; private set; }

        public string CustLevelName { get; private set; }

        public string DownLineLevelName { get; private set; }

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
                BreadcrumbLevelJson);

        public BreadcrumbLevelViewModel BreadcrumbLevelViewModel =>
           new BreadcrumbLevelViewModel(BreadcrumbLevel, BreadcrumbLevelJson);

        /// <summary>
        /// Get Drilldown Sorted Property.
        /// Return default Sorted Property if child level doesn't contain Sorted Property.
        /// </summary>
        /// <param name="custLevel"></param>
        /// <param name="sortedProperty"></param>
        /// <returns></returns>
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
                Title = Resources.MasterWinLoss;
                CustLevelName = Resources.Super;
                DownLineLevelName = Resources.Master;
            }
            else if (CustLevel.IsMasterLevel())
            {
                Title = Resources.AgentWinLoss;
                CustLevelName = Resources.Master;
                DownLineLevelName = Resources.Agent;
            }
            else
            {
                Title = Resources.MemberWinLoss;
                CustLevelName = Resources.Agent;
                DownLineLevelName = Resources.Member;
            }
        }

        protected override IDictionary<string, object> GetDownlineLinkRouteValues(WinLossModel model)
            => new Dictionary<string, object>
            {
                { "IsFilterVisible", IsFilterVisible },
                { "IsHistoricReport", IsHistoricReport },
                { "IsStackMode", IsStackMode },
                { "IsDescendingSort", IsDescendingSort },
                { "SortPropertyName", SortPropertyName },
                { "CustId", model.CustId },
                { "CustName", model.CustName },
                { "UserSelectedDateRangeOption", UserSelectedDateRangeOption },
                { "UserSelectedProductIds", UserSelectedProductIds },
                { "FromDate", FromDate },
                { "ToDate", ToDate }
            };

        protected override IDictionary<string, object> GetBetListLinkRouteValues(WinLossModel model)
            => new Dictionary<string, object>
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