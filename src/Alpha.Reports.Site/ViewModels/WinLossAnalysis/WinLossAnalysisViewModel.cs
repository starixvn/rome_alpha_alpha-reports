namespace Alpha.Reports.Site.ViewModels.WinLossAnalysis
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
    using Reports.Models.WinLossAnalysis;
    using Requests.WinLossAnalysis;

    public class WinLossAnalysisViewModel
    {    
        private readonly WinLossAnalysisQuery message;
        private readonly WinLossAnalysisParam param;
        public readonly string SortPropertyNameDefault = nameof(WinLossAnalysisReportTypeViewModel.ReportTypeName);

        public WinLossAnalysisViewModel(
            IEnumerable<WinLossAnalysisModel> reportData,
            WinLossAnalysisModel grandTotal,
            WinLossAnalysisQuery message,
            WinLossAnalysisParam param,
            IEnumerable<SelectListItem> products,
            IEnumerable<SelectListItem> excludeProducts)
        {
            this.message = message;
            this.param = param;

            Products = products;
            ExcludeProducts = excludeProducts;
            ReportData = reportData;
            GrandTotal = grandTotal;
        }

        public int CustId => message.CustId;

        public string CustName => message.CustName;

        public UserLevel CustLevel => message.CustLevel;

        public DateTime MinDate => message.MinDate;

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => param.FromDate;

        public DateTime ToDate => param.ToDate;

        public string Language => message.Language;

        public string UserSelectedProductIds
            => param.UserSelectedProductIds.Concat(param.UserSelectedExcludeProductIds).JoinWithComma();

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public IEnumerable<WinLossAnalysisModel> ReportData { get; }

        public WinLossAnalysisModel GrandTotal { get; }

        public IEnumerable<WinLossAnalysisReportTypeViewModel> ReportTypeData => BuildReportTypeData();

        public int ReportTypeId => message.ReportTypeId;

        public bool IsSetDefaultStackMode => message.IsSetDefaultStackMode;

        public bool IsStackMode => message.IsStackMode;

        public string SortPropertyName => GetSortPropertyName();

        public bool IsDescendingSort => message.IsDescendingSort;

        public bool IsFilterVisible => message.IsFilterVisible;

        public IEnumerable<SelectListItem> Products { get; }

        public IEnumerable<SelectListItem> ExcludeProducts { get; }

        public string ReportTypeName => ReportTypeData.FirstOrDefault(reportType => reportType.IsChecked.Equals(true)).ReportTypeName;

        public string Title => ReportTypeData.FirstOrDefault(reportType => reportType.IsChecked.Equals(true)).ReportTypeTitlte;

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        private ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(Products, UserSelectedProductIds, Language);

        private ExcludeProductViewModel ExcludeProductViewModel =>
            new ExcludeProductViewModel(ExcludeProducts, UserSelectedExcludeProductIds);

        public string UserSelectedExcludeProductIds => param.UserSelectedExcludeProductIds.JoinWithComma();

        public WinLossAnalysislFilterViewModel FilterViewModel =>
            new WinLossAnalysislFilterViewModel(
                CustId,
                CustName,
                CustLevel,
                DateRangePickerViewModel,
                ProductMultiSelectionViewModel,
                ExcludeProductViewModel,
                IsFilterVisible,
                MinDate,
                ReportTypeData);

        private string GetSortPropertyName()
        {
            if (message.SortPropertyName.IsNullOrBlank())
            {
                return ReportTypeId.Equals(WinLossAnalysisConstants.Sport) ? string.Empty : SortPropertyNameDefault;
            }

            return message.SortPropertyName;
        }

        private IEnumerable<WinLossAnalysisReportTypeViewModel> BuildReportTypeData()
        {
            var reportTypeData = new List<WinLossAnalysisReportTypeViewModel>()
            {
                new WinLossAnalysisReportTypeViewModel(WinLossAnalysisConstants.Daily, Resources.Daily, false, Resources.DailyWinLossAnalysis),
                new WinLossAnalysisReportTypeViewModel(WinLossAnalysisConstants.Monthly, Resources.Monthly, false, Resources.MonthlyWinLossAnalysis),
                new WinLossAnalysisReportTypeViewModel(WinLossAnalysisConstants.Sport, Resources.Sport, false, Resources.SportWinLossAnalysis)
            };

            foreach (var reportType in reportTypeData.Where(data => data.ReportTypeId.Equals(message.ReportTypeId)))
            {
                reportType.IsChecked = true;             
            }

            return reportTypeData;
        }
    }
}