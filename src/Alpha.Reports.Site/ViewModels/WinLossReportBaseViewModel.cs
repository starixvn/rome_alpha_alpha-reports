namespace Alpha.Reports.Site.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Components;
    using Extensions;
    using Params;
    using Reports.Constants;
    using Reports.Models;
    using Requests.WinLoss;
    using WinLoss;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class WinLossReportBaseViewModel<TReportData> : DrillDownableReportViewModel<TReportData>
        where TReportData : class
    {
        protected readonly WinLossQuery message;
        protected readonly WinLossParam param;

        public WinLossReportBaseViewModel(
            WinLossQuery message,
            WinLossParam param,
            IEnumerable<CustInfo> breadcrumbLevel,
            IEnumerable<GroupProductViewModel> groupProducts) : base(message.CustLevel, breadcrumbLevel)
        {
            this.message = message;
            this.param = param;
            GroupProducts = groupProducts;
        }

        public int CustId => message.CustId;

        public string CustName => message.CustName;

        public DateTime MinDate => message.MinDate;

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => param.FromDate;

        public DateTime ToDate => param.ToDate;

        public string Language => message.Language;

        public string UserSelectedProductIds =>
            param.UserSelectedProductIds.JoinWithComma();

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public bool IsSetDefaultStackMode => message.IsSetDefaultStackMode;

        public bool IsStackMode => message.IsStackMode;

        public bool IsHistoricReport => message.IsHistoricReport;

        public bool IsFilterVisible => message.IsFilterVisible;

        public IEnumerable<GroupProductViewModel> GroupProducts { get; }

        public IEnumerable<SelectListItem> ExcludeProducts { get; }

        private MonthPickerViewModel MonthPickerViewModel =>
            new MonthPickerViewModel(MinDate, FromDate, ToDate);

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        private ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(GroupProducts, UserSelectedProductIds, Language);

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

        protected string GetSortedProperty(UserLevel custLevel, string sortedProperty)
        {
            if (string.IsNullOrWhiteSpace(sortedProperty))
            {
                return DefaultVariables.SortPropertyName;
            }

            var isSortedPropertyInRange =
                custLevel.IsSuperLevel()
                || (custLevel.IsMasterLevel() && !sortedProperty.StartsWith("Super", StringComparison.OrdinalIgnoreCase))
                || (custLevel.IsAgentLevel()
                        && !sortedProperty.StartsWith("Super", StringComparison.OrdinalIgnoreCase)
                        && !sortedProperty.StartsWith("Master", StringComparison.OrdinalIgnoreCase));

            return isSortedPropertyInRange ? sortedProperty : DefaultVariables.SortPropertyName;
        }

        protected override IDictionary<string, object> GetDownlineLinkRouteValues(TReportData model)
            => new Dictionary<string, object>
            {
                { "IsFilterVisible", IsFilterVisible },
                { "IsHistoricReport", IsHistoricReport },
                { "IsStackMode", IsStackMode },
                { "UserSelectedDateRangeOption", UserSelectedDateRangeOption },
                { "UserSelectedProductIds", UserSelectedProductIds },
                { "FromDate", FromDate },
                { "ToDate", ToDate }
            };

        protected override IDictionary<string, object> GetBetListLinkRouteValues(TReportData model)
            => new Dictionary<string, object>
            {
                { "FromDate",  FromDate },
                { "ToDate", ToDate },
                { "ProductIds", UserSelectedProductIds }
            };
    }
}