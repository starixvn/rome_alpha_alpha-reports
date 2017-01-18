namespace Alpha.Reports.Site.ViewModels.MatchWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using App_GlobalResources;
    using Components;
    using Params;
    using Requests.MatchWinLoss;

    public class MatchWinLossReportViewModel : DrillDownableReportViewModel<MatchWinLossViewModel>
    {
        private readonly MatchWinLossParam matchWinLossParam;

        public MatchWinLossReportViewModel(
            MatchWinLossQuery matchWinLossQuery,
            DateTime minDate,
            MatchWinLossParam matchWinLossParam,
            IEnumerable<MatchWinLossViewModel> reportData,
            MatchWinLossViewModel grandTotal) : base(matchWinLossQuery.CustLevel, null)
        {
            UserSelectedDateRangeOption = matchWinLossQuery.UserSelectedDateRangeOption;
            CustId = matchWinLossQuery.CustId;
            IsFilterVisible = matchWinLossQuery.IsFilterVisible;

            MinDate = minDate;
            this.matchWinLossParam = matchWinLossParam;
            ReportData = reportData;
            GrantTotal = grandTotal;

            var canSort = reportData != null && reportData.Count() > 1;
            SortPropertyName = canSort ? matchWinLossQuery.SortPropertyName : "EventInfo";
            IsDescendingSort = canSort && matchWinLossQuery.IsDescendingSort;
        }

        public string Title => Resources.MatchWinLoss;

        public DateTime MinDate { get; }

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => matchWinLossParam.FromDate;

        public DateTime ToDate => matchWinLossParam.ToDate;

        public int UserSelectedDateRangeOption { get; }

        public int CustId { get; }

        public bool IsFilterVisible { get; }

        public string SortPropertyName { get; }

        public bool IsDescendingSort { get; }

        public IEnumerable<MatchWinLossViewModel> ReportData { get; }

        public MatchWinLossViewModel GrantTotal { get; }

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        public MatchWinLossFilterViewModel FilterViewModel =>
          new MatchWinLossFilterViewModel(CustId, CustLevel, DateRangePickerViewModel, IsFilterVisible, MinDate);

        protected override IDictionary<string, object> GetDownlineLinkRouteValues(MatchWinLossViewModel model)
        {
            throw new NotImplementedException();
        }

        protected override IDictionary<string, object> GetBetListLinkRouteValues(MatchWinLossViewModel model)
            => new Dictionary<string, object>
            {
                { "MatchId", model.MatchId },
                { "BetType", model.BetTypeId },
                { "type",  "WLByBettypeMatch" },
                { "islive", model.LiveIndicator ? "1" : "0" }
            };
    }
}