namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Components;
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
            IEnumerable<SelectListItem> currencies,
            IEnumerable<T> reportData)
        {
            this.message = message;
            this.param = param;

            Products = products;
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

        public string UserSelectedProductIds => param.ProductIds.JoinWithComma();

        public int UserSelectedCurrencyId => message.UserSelectedCurrencyId;

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public bool IsStackMode => message.IsStackMode;

        public bool IsFilterVisible => message.IsFilterVisible;

        public bool IsSetDefaultStackMode => message.IsSetDefaultStackMode;

        public UserLevel CustLevel => message.CustLevel;

        public IEnumerable<SelectListItem> Products { get; }

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
              CurrencySelectionViewModel);

        public string BuildDrillDownLink(
            string drilldownLink,
            string drilldownFromType,
            string drilldownFromProductIds,
            DateTime? drilldownDate,
            ConsolidationByUsernameViewModel data)
        {
            return ReportHelpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc(
                 drilldownLink,
                 new Dictionary<string, object>
                 {
                     ["Language"] = Language,
                     ["CustId"] = data.CustId,
                     ["CustName"] = data.Username,
                     ["CustLevelId"] = data.CustLevel,

                     ["UserSelectedCurrencyId"] = UserSelectedCurrencyId,
                     ["UserSelectedProductIds"] = UserSelectedProductIds,
                     ["IsStackMode"] = IsStackMode,
                     ["IsFilterVisible"] = IsFilterVisible,
                     ["UserSelectedDateRangeOption"] = UserSelectedDateRangeOption,
                     ["FromDate"] = FromDate,
                     ["ToDate"] = ToDate,

                     ["DrilldownFromType"] = drilldownFromType,
                     ["DrilldownFromProductIds"] = drilldownFromProductIds ?? UserSelectedProductIds,
                     ["DrilldownDate"] = drilldownDate
                 });
        }
    }
}