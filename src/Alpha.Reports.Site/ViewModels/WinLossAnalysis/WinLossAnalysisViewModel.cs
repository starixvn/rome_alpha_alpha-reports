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
        private readonly string sortPropertyNameDefault = "ReportTypeValue";
        private readonly WinLossAnalysisQuery message;
        private readonly WinLossAnalysisParam winLossParam;

        public WinLossAnalysisViewModel(
            IEnumerable<WinLossAnalysisModel> reportData,
            WinLossAnalysisModel grandTotal,
            WinLossAnalysisQuery message,
            WinLossAnalysisParam winLossParam,
            IEnumerable<SelectListItem> products)
        {
            this.message = message;
            this.winLossParam = winLossParam;

            Products = products;
            ReportData = reportData;
            GrandTotal = grandTotal;
        }

        public int CustId => message.CustId;

        public string CustName => message.CustName;

        public UserLevel CustLevel => message.CustLevel;

        public DateTime MinDate => message.MinDate;

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => winLossParam.FromDate;

        public DateTime ToDate => winLossParam.ToDate;

        public string Language => message.Language;

        public string UserSelectedProductIds => winLossParam.UserSelectedProductIds.JoinWithComma();

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public IEnumerable<WinLossAnalysisModel> ReportData { get; }

        public WinLossAnalysisModel GrandTotal { get; }

        public IEnumerable<WinLossAnalysisReportType> ReportTypeData => BuildReportTypeData();

        public bool IsSetDefaultStackMode => message.IsSetDefaultStackMode;

        public bool IsStackMode => message.IsStackMode;

        public string SortPropertyName => sortPropertyNameDefault;

        public bool IsDescendingSort => message.IsDescendingSort;

        public bool IsFilterVisible => message.IsFilterVisible;

        public IEnumerable<SelectListItem> Products { get; }

        public string ReportTypeName => ReportTypeData.FirstOrDefault(reportType => reportType.IsChecked.Equals(true)).ReportTypeName;

        public string Title => Resources.WinLossAnalysis;

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        private ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(Products, UserSelectedProductIds, Language);

        public WinLossAnalysislFilterViewModel FilterViewModel =>
            new WinLossAnalysislFilterViewModel(
                CustId,
                CustName,
                CustLevel,
                message.ReportTypeId,
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