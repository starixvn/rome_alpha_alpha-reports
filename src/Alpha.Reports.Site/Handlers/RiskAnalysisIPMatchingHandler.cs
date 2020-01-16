namespace Alpha.Reports.Site.Handlers
{
    using Alpha.Reports.Params;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Requests;
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
                message.WinlostDate,
                riskAnalysisIPMatchingService.GetRiskAnalysisIPMatchingModel(
                    new RiskAnalysisIPMatchingParam(
                        message.CustId, message.WinlostDate, message.MoveBetsDate, message.MemberCount)));
    }
}