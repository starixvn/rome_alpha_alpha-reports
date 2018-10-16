namespace Alpha.Reports.Site.ViewModels.WinLossAnalysisChart
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Params;
    using Reports.Models;
    using Reports.Models.WinLossAnalysisChart;
    using Requests.WinLossAnalysisChart;

    public class WinLossAnalysisChartViewModel
    {
        private readonly WinLossAnalysisChartQuery message;
        private readonly WinLossAnalysisChartParam param;

        public WinLossAnalysisChartViewModel(
            IEnumerable<WinLossAnalysisChartModel> reportData,
            WinLossAnalysisChartQuery message,
            WinLossAnalysisChartParam param,
            IEnumerable<SelectListItem> products,
            IEnumerable<SelectListItem> excludeProducts)
        {
            this.message = message;
            this.param = param;

            Products = products;
            ExcludeProducts = excludeProducts;
            ReportData = reportData;

            Title = Resources.MonthlyWinLossChart;
        }

        public string Title { get; }

        public int CustId => message.CustId;

        public string CustName => message.CustName;

        public UserLevel CustLevel => message.CustLevel;

        public DateTime MinDate => message.MinDate;

        public DateTime FromDate => param.FromDate;

        public DateTime ToDate => param.ToDate;

        public string Language => message.Language;

        public string UserSelectedProductIds
            => param.UserSelectedProductIds.Concat(param.UserSelectedExcludeProductIds).JoinWithComma();

        public bool IsFilterVisible => message.IsFilterVisible;

        public IEnumerable<WinLossAnalysisChartModel> ReportData { get; }

        public IEnumerable<SelectListItem> Products { get; }

        public IEnumerable<SelectListItem> ExcludeProducts { get; }

        private ExcludeProductViewModel ExcludeProductViewModel =>
            new ExcludeProductViewModel(ExcludeProducts, UserSelectedExcludeProductIds);

        public string UserSelectedExcludeProductIds => param.UserSelectedExcludeProductIds.JoinWithComma();

        private ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(Products, UserSelectedProductIds, Language);

        public MonthPickerViewModel MonthPickerViewModel =>
            new MonthPickerViewModel(MinDate, FromDate, ToDate);

        public WinLossAnalysisChartFilterViewModel FilterViewModel =>
            new WinLossAnalysisChartFilterViewModel(
                CustId,
                CustName,
                CustLevel,
                ProductMultiSelectionViewModel,
                ExcludeProductViewModel,
                MonthPickerViewModel,
                IsFilterVisible,
                MinDate);

        public DualAxisChartViewModel DualAxisChartViewModel =>
            new DualAxisChartViewModel(
                $"{Resources.MonthlyWinLossChart} {Resources.From} {FromDate.ToShortDateWithSlash()} {Resources.To} {ToDate.ToShortDateWithSlash()}",
                ReportData.Select(x => x.WinLossDate.ToShortMonthNameAndShortYear()).JoinWithComma(),
                $"{CustLevel.DisplayName} {Resources.WinLossTitle}",
                Resources.Turnover,
                ReportData.Select(x => x.WinLoss).JoinWithComma(),
                ReportData.Select(x => x.Turnover).JoinWithComma(),
                message.CurrencyName);
    }
}