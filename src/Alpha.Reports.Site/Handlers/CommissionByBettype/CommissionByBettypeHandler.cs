namespace Alpha.Reports.Site.Handlers.CommissionByBettype
{
    using MediatR;
    using Params;
    using Requests.CommissionByBettype;
    using Services;
    using Sunplus.Agent.Model.Definitions;
    using ViewModels.CommissionByBettype;

    public class CommissionByBettypeHandler : BaseHandler, IRequestHandler<CommissionByBettypeQuery, CommissionByBettypeViewModel>
    {
        private CommissionByBettypeService commissionService;
        private ReportDateService reportDateService;
        private UserPermissionService userPermissionService;

        public CommissionByBettypeHandler(
            CommissionByBettypeService commissionByBettypeServiceImpl, 
            ReportDateService reportDateServiceImpl, 
            UserPermissionService userPermissionServiceImpl)
        {
            commissionService = commissionByBettypeServiceImpl;
            reportDateService = reportDateServiceImpl;
            userPermissionService = userPermissionServiceImpl;
        }

        public CommissionByBettypeViewModel Handle(CommissionByBettypeQuery message)
        {
            var param = new CommissionByBettypeParam(
                message.ViewerLevel, 
                message.CustLevel, 
                message.CustId, 
                message.FromDate, 
                message.ToDate, 
                reportDateService.GetMoveBetsDay());

            var commissions = commissionService.GetAllCommissionByBettypes(param);

            var breadcrumbLevel = BuildBreadcrumbLevel(message);

            return new CommissionByBettypeViewModel(
                message,
                commissions,
                breadcrumbLevel,
                reportDateService.GetBalanceUpDay(),
                userPermissionService.IsIMbcCustomer(),
                userPermissionService.IsShowMyanmarOdds(),
                userPermissionService.IsEnableProduct(ProductIdConstants.CockFighting));
        }
    }
}