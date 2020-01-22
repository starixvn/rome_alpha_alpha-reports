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
        public RiskAnalysisIPMatchingQuery(IUser viewer) : base(viewer)
        {
            SelectedWinlostDate = SystemTime.Today;
        }

        public int MemberCount { get; set; }

        public DateTime SelectedWinlostDate { get; set; }

        public DateTime MinDate => ReportDateServiceImpl.Instance.GetBalanceUpDay();

        public DateTime MoveBetsDate => ReportDateServiceImpl.Instance.GetMoveBetsDay();

        public override bool IsValid() => base.IsValid() && MinDate <= SelectedWinlostDate;
    }
}