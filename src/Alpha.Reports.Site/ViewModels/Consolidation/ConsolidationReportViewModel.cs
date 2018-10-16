namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            IEnumerable<SelectListItem> products,
            IEnumerable<SelectListItem> excludeProducts,
            IEnumerable<SelectListItem> currencies,
            IEnumerable<T> reportData)
        {
            this.message = message;
            this.param = param;

            Products = products;
            ExcludeProducts = excludeProducts;
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
            => param.ProductIds.Concat(param.ExcludeProductIds).JoinWithComma();

        public int UserSelectedCurrencyId => message.UserSelectedCurrencyId;

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public bool IsStackMode => message.IsStackMode;

        public bool IsFilterVisible => message.IsFilterVisible;

        public bool IsSetDefaultStackMode => message.IsSetDefaultStackMode;

        public UserLevel CustLevel => message.CustLevel;

        public IEnumerable<SelectListItem> Products { get; }

        public IEnumerable<SelectListItem> ExcludeProducts { get; }

        private ExcludeProductViewModel ExcludeProductViewModel =>
            new ExcludeProductViewModel(ExcludeProducts, UserSelectedExcludeProductIds);

        public string UserSelectedExcludeProductIds => param.ExcludeProductIds.JoinWithComma();

        public IEnumerable<SelectListItem> Currencies { get; }

        public IEnumerable<T> ReportData { get; }

        private DateRangePickerViewModel DateRangePickerViewModel =>
          new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        private ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(Products, UserSelectedProductIds, Language);

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
              CurrencySelectionViewModel,
              ExcludeProductViewModel);

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

                ["IsStackMode"] = IsStackMode,
                ["IsFilterVisible"] = IsFilterVisible,
                ["UserSelectedDateRangeOption"] = userSelectedDateRangeOption,
                ["FromDate"] = drilldownDate ?? FromDate,
                ["ToDate"] = drilldownDate ?? ToDate,

                ["DrilldownFromType"] = drilldownFromType,
                ["ConsolidationFromDate"] = FromDate,
                ["ConsolidationToDate"] = ToDate,
                ["ConsolidationUserSelectedDateRangeOption"] = UserSelectedDateRangeOption,
                ["ConsolidationProductIds"] = UserSelectedProductIds
            };

            if (drilldownFromProductIds == null
                || drilldownFromProductIds.Equals(UserSelectedExcludeProductIds))
            {
                routeValues.Add(nameof(UserSelectedExcludeProductIds), UserSelectedExcludeProductIds);
            }

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
                ["FilterPostback"] = 1,
                ["CustId"] = data.CustId,
                ["CustName"] = data.Username,
                ["IsGroupHeadline"] = true
            };

            return ReportHelpers.UrlHelper.AddQueryStringParamsFunc(ReportHelpers.UrlHelper.MapBetListPath("~/WinLoss/Index"), routeValues);
        }
    }
}