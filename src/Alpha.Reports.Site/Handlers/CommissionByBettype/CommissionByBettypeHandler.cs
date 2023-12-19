namespace Alpha.Reports.Site.Handlers.CommissionByBettype
{
    using System.Collections.Generic;
    using System.Linq;
    using App_GlobalResources;
    using MediatR;
    using Params;
    using Requests.CommissionByBettype;
    using Services;
    using ViewModels.CommissionByBettype;

    public class CommissionByBettypeHandler : BaseHandler,
        IRequestHandler<CommissionByBettypeQuery, CommissionByBettypeViewModel>,
        IRequestHandler<CommissionByBetTypeGuideQuery, IEnumerable<string>>
    {
        private readonly CommissionByBettypeService commissionService;
        private readonly ReportDateService reportDateService;
        private readonly UserPermissionService userPermissionService;
        private readonly ProductService productService;

        public CommissionByBettypeHandler(
            CommissionByBettypeService commissionByBettypeServiceImpl,
            ReportDateService reportDateServiceImpl,
            UserPermissionService userPermissionServiceImpl,
            ProductService productServiceImpl)
        {
            productService = productServiceImpl;
            commissionService = commissionByBettypeServiceImpl;
            userPermissionService = userPermissionServiceImpl;
            reportDateService = reportDateServiceImpl;
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
                userPermissionService.IsShowMyanmarOdds());
        }

        public IEnumerable<string> Handle(CommissionByBetTypeGuideQuery message)
        {
            var result = new List<string>();

            var productsNeedDelayTimeToSumarizeData = productService.GetOfferedProductsNeedDelayTimeToSumarizeData(message.Site, message.CurrencyId, message.MinDate).Select(product => product);

            if (productsNeedDelayTimeToSumarizeData.Any())
            {
                result.Add(string.Format(
                    Resources.DelayIssueLegend,
                    string.Join(", ", productService.GetProductNames(productsNeedDelayTimeToSumarizeData))));
            }

            return result;
        }
    }
}