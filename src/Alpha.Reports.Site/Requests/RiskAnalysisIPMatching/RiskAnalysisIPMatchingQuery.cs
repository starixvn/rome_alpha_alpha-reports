namespace Alpha.Reports.Site.Requests.RiskAnalysisIPMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.ViewModels;
    using Alpha.Reports.Utils;
    using MediatR;
    using System;

    public class RiskAnalysisIPMatchingQuery : ReportBaseQuery, IRequest<RiskAnalysisIPMatchingViewModel>
    {
        private readonly int defaultMemberCount = 2;

        public RiskAnalysisIPMatchingQuery(IUser viewer) : base(viewer)
        {
            MemberCount = defaultMemberCount;
            SelectedWinlostDate = SystemTime.Today;
            MinDate = ReportDateServiceImpl.Instance.GetBalanceUpDay();
            MoveBetsDate = ReportDateServiceImpl.Instance.GetMoveBetsDay();

        }

        public int MemberCount { get; set; }

        public DateTime SelectedWinlostDate { get; set; }

        public DateTime MinDate { get; }

        public DateTime MoveBetsDate { get; }

        public override bool IsValid() => base.IsValid() && MinDate <= SelectedWinlostDate && ViewerLevel.IsSuperLevel();
    }
}