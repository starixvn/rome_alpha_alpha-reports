namespace Alpha.Reports.Site.Handlers
{
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Requests.RiskAnalysisIPMatching;
    using Alpha.Reports.Site.ViewModels;
    using Alpha.Utilities.IP;
    using MediatR;

    public class RiskAnalysisIPMatchingHandler :
        RiskAnalysisIPMatchingBaseHandler,
        IRequestHandler<RiskAnalysisIPMatchingQuery, RiskAnalysisIPMatchingViewModel>
    {
        public RiskAnalysisIPMatchingHandler(
            RiskAnalysisIPMatchingService riskAnalysisIPMatchingService,
            IPService ipService) 
            : base(riskAnalysisIPMatchingService, ipService)
        {
        }

        public RiskAnalysisIPMatchingViewModel Handle(RiskAnalysisIPMatchingQuery message)
            => BuildRiskAnalysisIPMatchingViewModel(message);
    }
}