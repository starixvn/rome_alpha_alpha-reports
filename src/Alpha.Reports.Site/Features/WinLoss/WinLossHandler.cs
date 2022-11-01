namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using MediatR;
    using Requests.WinLoss;
    using Services;
    using ViewModels.WinLoss;

    public class WinLossHandler :
        WinLossBaseHandler,
        IRequestHandler<WinLossQuery, WinLossViewModel>
    {
        public WinLossHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossService reportService) : base(productService, productDetailService, reportService)
        {
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public WinLossViewModel Handle(WinLossQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var products = productService.GetProductsForCustomer(message.ViewerId, message.Site, message.CurrencyId, message.MinDate);
            var productDetails = productDetailService.GetProductDetails(message.ViewerId, message.Site, message.CurrencyId);

            var winLossParam = CreateWinLossParamater(message);
            var reportData = reportService.GetWinLoss(winLossParam);
            var grandTotal = AccumulateGrandTotal(reportData);

            var viewModel = new WinLossViewModel(
                reportData,
                grandTotal,
                message,
                winLossParam,
                BuildBreadcrumbLevel(message),
                BuildProductSelectListItems(winLossParam.UserSelectedProductIds, products),
                BuildProductDetailViewModels(winLossParam.UserSelectedResourceIds, productDetails));

            return viewModel;
        }
    }
}