namespace Alpha.Reports.Site.Requests
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
            WinlostDate = SystemTime.Today;
        }

        public int MemberCount { get; set; }

        public DateTime WinlostDate { get; set; }

        public DateTime MinDate => ReportDateServiceImpl.Instance.GetBalanceUpDay();

        public DateTime MoveBetsDate => ReportDateServiceImpl.Instance.GetMoveBetsDay();

        public override bool IsValid() => base.IsValid() && MinDate <= WinlostDate;
    }
}