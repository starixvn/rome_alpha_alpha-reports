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
            IEnumerable<ProgressiveContributionByProductModel> progressiveContributions)
        {
            this.query = query;
            this.param = param;
            Products = products;
            ProgressiveContributions = progressiveContributions;
        }

        public string Title => Resources.ProgressiveGamesContribution;

        public string ViewDataInstructionMessage => string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, query.MinDate.ToShortDateWithSlash());

        public int CustId => param.CustId;

        public int CustLevelId => query.CustLevelId;

        public DateTime MinDate => query.MinDate;

        public static DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => param.FromDate;

        public DateTime ToDate => param.ToDate;

        public string Language => query.Language;

        public string UserSelectedProductIds => param.UserSelectedProductIds.JoinWithComma();

        public int UserSelectedDateRangeOption => query.UserSelectedDateRangeOption;

        public bool IsStackMode => query.IsStackMode;

        public bool IsFilterVisible => query.IsFilterVisible;

        public bool IsSetDefaultStackMode => query.IsSetDefaultStackMode;

        public UserLevel CustLevel => query.CustLevel;

        public int ContributionShareColumnsCount => CustLevel.IsSuperLevel() ? 4 : CustLevel.IsMasterLevel() ? 3 : 2;

        public int ReportColumnsCount => CustLevel.IsSuperLevel() ? 8 : CustLevel.IsMasterLevel() ? 7 : 6;

        public IEnumerable<SelectListItem> Products { get; }

        public IEnumerable<ProgressiveContributionByProductModel> ProgressiveContributions { get; }

        public DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(Products, UserSelectedProductIds, Language);
    }
}