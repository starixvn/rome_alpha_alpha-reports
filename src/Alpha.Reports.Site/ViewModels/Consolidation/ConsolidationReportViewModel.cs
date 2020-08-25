namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Components;
    using Constants;
    using Extensions;
    using Params.Consolidation;
    using Reports.Models;
    using Requests.Consolidation;
    using ReportHelpers = Helpers;

    public class ConsolidationReportViewModel<T>
    {
        private readonly GetConsolidationQuery message;
        private readonly GetConsolidationParam param;

        public ConsolidationReportViewModel(
            GetConsolidationQuery message,
            GetConsolidationParam param,
            IEnumerable<GroupProductViewModel> groupProducts,
            IEnumerable<ProductDetailViewModel> productDetails,
            IEnumerable<SelectListItem> currencies,
            IEnumerable<T> reportData)
        {
            this.message = message;
            this.param = param;

            GroupProducts = groupProducts;
            ProductDetails = productDetails;
            Currencies = currencies;
            ReportData = reportData;
        }

        public int CustId => message.CustId;

        public string CustName => message.CustName;

        public DateTime MinDate => message.MinDate;

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => message.FromDate;

        public DateTime ToDate => message.ToDate;

        public string Language => message.Language;

        public string UserSelectedProductIds
            => param.ProductIds.JoinWithComma();

        public string UserSelectedResourceIds
            => param.ResourceIds.JoinWithComma();

        public int UserSelectedCurrencyId => message.UserSelectedCurrencyId;

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public bool IsStackMode => message.IsStackMode;

        public bool IsFilterVisible => message.IsFilterVisible;

        public bool IsSetDefaultStackMode => message.IsSetDefaultStackMode;

        public UserLevel CustLevel => message.CustLevel;

        public IEnumerable<GroupProductViewModel> GroupProducts { get; }

        public IEnumerable<ProductDetailViewModel> ProductDetails { get; }

        public IEnumerable<SelectListItem> Currencies { get; }

        public IEnumerable<T> ReportData { get; }

        private DateRangePickerViewModel DateRangePickerViewModel =>
          new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        private ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(GroupProducts, UserSelectedProductIds, Language);

        private ProductDetailMultiSelectionViewModel ProductDetailMultiSelectionViewModel =>
            new ProductDetailMultiSelectionViewModel(ProductDetails, UserSelectedResourceIds, Language);

        private CurrencySelectionViewModel CurrencySelectionViewModel =>
            new CurrencySelectionViewModel(Currencies, UserSelectedCurrencyId);

        public ConsolidationFilterViewModel FilterViewModel =>
          new ConsolidationFilterViewModel(
              CustId,
              CustName,
              IsFilterVisible,
              MinDate,
              CustLevel,
              DateRangePickerViewModel,
              ProductMultiSelectionViewModel,
              ProductDetailMultiSelectionViewModel,
              CurrencySelectionViewModel);

        public string BuildDrillDownLink(
            string drilldownLink,
            string drilldownFromType,
            string drilldownFromProductIds,
            DateTime? drilldownDate,
            ConsolidationByUsernameViewModel data)
        {
            if (data.CustLevel == UserLevel.Member.Value)
            {
                return BuildBetListLink(data, drilldownFromProductIds, drilldownDate);
            }

            var userSelectedDateRangeOption = string.Equals(drilldownFromType, "date", StringComparison.OrdinalIgnoreCase)
                ? UserSelectedDateRangeOptionConstant.SpecificDates
                : UserSelectedDateRangeOption;

            IDictionary<string, object> routeValues = new Dictionary<string, object>
            {
                ["Language"] = Language,
                ["CustId"] = data.CustId,
                ["CustName"] = data.Username,
                ["CustLevelId"] = data.CustLevel,

                ["UserSelectedCurrencyId"] = UserSelectedCurrencyId,
                ["UserSelectedProductIds"] = drilldownFromProductIds ?? UserSelectedProductIds,
                ["UserSelectedResourceIds"] = drilldownFromProductIds ?? UserSelectedResourceIds,

                ["IsStackMode"] = IsStackMode,
                ["IsFilterVisible"] = IsFilterVisible,
                ["UserSelectedDateRangeOption"] = userSelectedDateRangeOption,
                ["FromDate"] = drilldownDate ?? FromDate,
                ["ToDate"] = drilldownDate ?? ToDate,

                ["DrilldownFromType"] = drilldownFromType,
                ["ConsolidationFromDate"] = FromDate,
                ["ConsolidationToDate"] = ToDate,
                ["ConsolidationUserSelectedDateRangeOption"] = UserSelectedDateRangeOption,
                ["ConsolidationProductIds"] = UserSelectedProductIds,
                ["ConsolidationResourceIds"] = UserSelectedResourceIds
            };

            return ReportHelpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc(drilldownLink, routeValues);
        }

        private string BuildBetListLink(
            ConsolidationByUsernameViewModel data,
            string drilldownFromProductIds,
            DateTime? drilldownDate)
        {
            var routeValues = new Dictionary<string, object>
            {
                ["FromDate"] = drilldownDate ?? FromDate,
                ["ToDate"] = drilldownDate ?? ToDate,
                ["ProductIds"] = drilldownFromProductIds ?? UserSelectedProductIds,
                ["ResourceIds"] = drilldownFromProductIds ?? UserSelectedResourceIds,
                ["FilterPostback"] = 1,
                ["CustId"] = data.CustId,
                ["CustName"] = data.Username,
                ["IsGroupHeadline"] = true
            };

            return ReportHelpers.UrlHelper.AddQueryStringParamsFunc(ReportHelpers.UrlHelper.MapBetListPath("~/WinLoss/Index"), routeValues);
        }
    }
}