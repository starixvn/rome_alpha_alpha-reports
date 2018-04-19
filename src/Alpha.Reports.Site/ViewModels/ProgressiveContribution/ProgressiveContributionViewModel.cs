namespace Alpha.Reports.Site.ViewModels.ProgressiveContribution
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Params;
    using Reports.Models;
    using Reports.Models.ProgressiveContribution;
    using Requests.ProgressiveContribution;

    public class ProgressiveContributionViewModel
    {
        private readonly ProgressiveContributionQuery query;
        private readonly ProgressiveContributionParam param;

        public ProgressiveContributionViewModel(
            ProgressiveContributionQuery query,
            ProgressiveContributionParam param,
            IEnumerable<SelectListItem> products,
            ProgressiveContributionByBetTypeModel grandTotal,
            IEnumerable<ProgressiveContributionByProductModel> progressiveContributions)
        {
            this.query = query;
            this.param = param;
            Products = products;
            GrandTotal = grandTotal;
            ProgressiveContributions = progressiveContributions;
        }

        public string Title => Resources.ProgressiveGamesContribution;

        public string ViewDataInstructionMessage => string.Format(
            IsHistoricReport ? Resources.CanOnlyViewDataAfterTimeHistoryReport : Resources.CanOnlyViewDataAfterTimeCurrentReport,
            query.MinDate.ToShortDateWithSlash());

        public string ViewActionMessage => IsHistoricReport ? Resources.BackToCurrentReport : Resources.ViewHistoryReport;

        public int CustId => param.CustId;

        public int CustLevelId => query.CustLevelId;

        public DateTime MinDate => query.MinDate;

        public static DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => param.FromDate;

        public DateTime ToDate => param.ToDate;

        public string Language => query.Language;

        public string UserSelectedProductIds => param.UserSelectedProductIds.JoinWithComma();

        public int UserSelectedDateRangeOption => query.UserSelectedDateRangeOption;

        public bool IsHistoricReport => query.IsHistoricReport;

        public bool IsStackMode => query.IsStackMode;

        public bool IsFilterVisible => query.IsFilterVisible;

        public bool IsSetDefaultStackMode => query.IsSetDefaultStackMode;

        public UserLevel CustLevel => query.CustLevel;

        public int ContributionShareColumnsCount => CustLevel.IsSuperLevel() ? 3 : CustLevel.IsMasterLevel() ? 2 : 1;

        public int ReportColumnsCount => CustLevel.IsSuperLevel() ? 8 : CustLevel.IsMasterLevel() ? 7 : 6;

        public IEnumerable<SelectListItem> Products { get; }

        public IEnumerable<ProgressiveContributionByProductModel> ProgressiveContributions { get; }

        public ProgressiveContributionByBetTypeModel GrandTotal { get; }

        public DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        public MonthPickerViewModel MonthPickerViewModel =>
            new MonthPickerViewModel(MinDate, FromDate, ToDate);

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(Products, UserSelectedProductIds, Language);
    }
}