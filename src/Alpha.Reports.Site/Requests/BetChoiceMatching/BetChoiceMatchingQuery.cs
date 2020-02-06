namespace Alpha.Reports.Site.Requests.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.ViewModels.BetChoiceMatching;
    using Alpha.Reports.Utils;
    using MediatR;
    using System;

    public class BetChoiceMatchingQuery : ReportBaseQuery, IRequest<BetChoiceMatchingQueryResult>
    {
        public BetChoiceMatchingQuery(IUser viewer) : base(viewer)
        {
            WinlostDate = SystemTime.Today;
        }

        public int BetCount { get; set; }

        public DateTime WinlostDate { get; set; }

        public DateTime MinDate { get; } = ReportDateServiceImpl.Instance.GetBalanceUpDay();

        public DateTime MoveBetsDate { get; } = ReportDateServiceImpl.Instance.GetMoveBetsDay();

        public override bool IsValid() => base.IsValid() && BetChoiceMatchingViewModel.DefaultBetCounts.Contains(BetCount) && MinDate <= WinlostDate;
    }
}