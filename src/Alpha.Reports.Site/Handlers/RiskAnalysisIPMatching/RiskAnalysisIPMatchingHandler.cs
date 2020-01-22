namespace Alpha.Reports.Site.Handlers
{
    using Alpha.Reports.Params;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Requests.RiskAnalysisIPMatching;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;

    public class RiskAnalysisIPMatchingHandler : 
        BaseHandler,
        IRequestHandler<RiskAnalysisIPMatchingQuery, RiskAnalysisIPMatchingViewModel>
    {
        protected readonly RiskAnalysisIPMatchingService riskAnalysisIPMatchingService;

        public RiskAnalysisIPMatchingHandler(RiskAnalysisIPMatchingService riskAnalysisIPMatchingService)
        {
            this.riskAnalysisIPMatchingService = riskAnalysisIPMatchingService;
        }

        public RiskAnalysisIPMatchingViewModel Handle(RiskAnalysisIPMatchingQuery message)
            => new RiskAnalysisIPMatchingViewModel(
                message.SelectedWinlostDate,
                message.MinDate,
                message.MemberCount,
                riskAnalysisIPMatchingService.GetRiskAnalysisIPMatchingModel(
                    new RiskAnalysisIPMatchingParam(
                        message.CustId, message.SelectedWinlostDate, message.MoveBetsDate, message.MemberCount)));
    }
}