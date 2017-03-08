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
        public readonly string SortPropertyNameDefault = nameof(WinLossAnalysisReportType.ReportTypeName);

        public WinLossAnalysisViewModel(
            IEnumerable<WinLossAnalysisModel> reportData,
            WinLossAnalysisModel grandTotal,
            WinLossAnalysisQuery message,
            WinLossAnalysisParam param,
            IEnumerable<SelectListItem> products)
        {
            this.message = message;
            this.param = param;

            Products = products;
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

        public string UserSelectedProductIds => param.UserSelectedProductIds.JoinWithComma();

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public IEnumerable<WinLossAnalysisModel> ReportData { get; }

        public WinLossAnalysisModel GrandTotal { get; }

        public IEnumerable<WinLossAnalysisReportType> ReportTypeData => BuildReportTypeData();

        public int ReportTypeId => message.ReportTypeId;

        public bool IsSetDefaultStackMode => message.IsSetDefaultStackMode;

        public bool IsStackMode => message.IsStackMode;

        public string SortPropertyName => message.SortPropertyName.IsNullOrBlank() ? SortPropertyNameDefault : message.SortPropertyName;

        public bool IsDescendingSort => message.IsDescendingSort;

        public bool IsFilterVisible => message.IsFilterVisible;

        public IEnumerable<SelectListItem> Products { get; }

        public string ReportTypeName => ReportTypeData.FirstOrDefault(reportType => reportType.IsChecked.Equals(true)).ReportTypeName;

        public string Title => $"{ReportTypeName} {Resources.WinLossAnalysis}";

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        private ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(Products, UserSelectedProductIds, Language);

        public WinLossAnalysislFilterViewModel FilterViewModel =>
            new WinLossAnalysislFilterViewModel(
                CustId,
                CustName,
                CustLevel,
                DateRangePickerViewModel,
                ProductMultiSelectionViewModel,
                IsFilterVisible,
                MinDate,
                ReportTypeData);

        private IEnumerable<WinLossAnalysisReportType> BuildReportTypeData()
        {
            var reportTypeData = new List<WinLossAnalysisReportType>()
            {
                new WinLossAnalysisReportType(1, Resources.Daily, false),
                new WinLossAnalysisReportType(2, Resources.Monthly, false),
                new WinLossAnalysisReportType(3, Resources.Sport, false)
            };

            foreach (var reportType in reportTypeData.Where(data => data.ReportTypeId.Equals(message.ReportTypeId)))
            {
                reportType.IsChecked = true;             
            }

            return reportTypeData;
        }
    }
}