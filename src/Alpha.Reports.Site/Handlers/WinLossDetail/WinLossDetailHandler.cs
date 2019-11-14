namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using App_GlobalResources;
    using MediatR;
    using Reports.Constants;
    using Reports.Models;
    using Requests.WinLossDetail;
    using Services;
    using ViewModels.WinLossDetail;

    public class WinLossDetailHandler :
        WinLossDetailBaseHandler,
        IRequestHandler<WinLossDetailQuery, WinLossDetailViewModel>,
        IRequestHandler<ReportLegendQuery, IEnumerable<string>>
    {
        private const int AllBetProductId = 21;
        private readonly IUser viewer;

        public WinLossDetailHandler(
            ProductService productService,
            WinLossDetailService reportService,
            IUser viewer) : base(productService, reportService)
        {
            this.viewer = viewer;
        }

        public WinLossDetailViewModel Handle(WinLossDetailQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var products = productService.GetProductsForCustomer(message.CustId, message.MinDate);

            var winLossDetailParam = CreateParameter(message, message.CustLevel);
            var reportData = reportService.GetWinLossDetail(winLossDetailParam);
            var viewModel = new WinLossDetailViewModel(
                message,
                winLossDetailParam,
                reportData,
                BuildBreadcrumbLevel(message),
                BuildProductSelectListItems(winLossDetailParam.UserSelectedProductIds, products));

            return viewModel;
        }

        public IEnumerable<string> Handle(ReportLegendQuery message)
        {
            message.CustId = viewer.Id;
            var result = new List<string>();
            result.Add(Resources.Tooltip_NetTurnover);

            var customerProducts = productService.GetProductsForCustomer(message.CustId);

            if (customerProducts.Any(product => Convert.ToInt32(product.Id) == AllBetProductId))
            {
                result.Add(Resources.Tooltip_NetTurnover_21);
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