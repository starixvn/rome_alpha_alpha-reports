namespace Alpha.Reports.Site.Requests.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.ViewModels.BetChoiceMatching;
    using Alpha.Reports.Utils;
    using MediatR;
    using System;

    public class BetChoiceMatchingJsonQuery : ReportBaseQuery, IRequest<BetChoiceMatchingJsonModel>
    {
        public BetChoiceMatchingJsonQuery(IUser viewer, ReportDateService reportDateService) : base(viewer)
        {
            WinlostDate = SystemTime.Today;
            var reportDate = reportDateService.GetBalanceUpDayAndMoveBetsDay();
            MinDate = reportDate.BalanceUpDay;
            MoveBetsDate = reportDate.MoveBetsDay;
        }

        public int BetCount { get; set; }

        public DateTime WinlostDate { get; set; }

        public DateTime MinDate { get; }

        public DateTime MoveBetsDate { get; }

        public override bool IsValid() => base.IsValid() && ViewerLevel.IsSuperLevel() && BetChoiceMatchingConstants.DefaultBetCounts.Contains(BetCount) && MinDate <= WinlostDate;
    }
}