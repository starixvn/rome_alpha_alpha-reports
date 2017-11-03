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
                userPermissionService.IsShowMyanmarOdds());
        }

        public IEnumerable<string> Handle(CommissionByBetTypeGuideQuery message)
        {
            var result = new List<string>();

            var productsInOthers = commissionService.GetProductsInOthers(message.CustId);
            if (productsInOthers.Any())
            {
                result.Add(string.Format(
                    Resources.Tooltip_CommissionByBetType,
                    string.Join(", ", productsInOthers.Select(i => i.DisplayName))));
            }

            var productsNeedDelayTimeToSumarizeData = productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer().Select(product => (int)product);

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