namespace Alpha.Reports.Site.Handlers.RacingCommission
{
    using MediatR;
    using Params;
    using Requests.WinLoss;
    using Services;
    using ViewModels.RacingCommission;

    public class RacingCommissionHandler : BaseHandler, IRequestHandler<RacingCommissionQuery, RacingCommissionViewModel>
    {
        private RacingCommissionService racingCommissionService;
        private ReportDateService reportDateService;

        public RacingCommissionHandler(RacingCommissionService racingCommissionService, ReportDateService reportDateService)
        {
            this.racingCommissionService = racingCommissionService;
            this.reportDateService = reportDateService;
        }

        public RacingCommissionViewModel Handle(RacingCommissionQuery message)
        {
            var param = new RacingCommissionParam(message.ViewerLevel, message.CustLevel, message.CustId, message.FromDate, message.ToDate);

            var commissions = racingCommissionService.GetCustomerCommission(param);

            return new RacingCommissionViewModel(message, commissions, reportDateService.GetBalanceUpDay());
        }
    }
}