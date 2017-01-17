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
            WinLossService reportService) : base(productService, reportService)
        {
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public WinLossViewModel Handle(WinLossQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var products = productService.GetProductsForCustomer(message.CustId);

            var winLossParam = CreateWinLossParamater(message);
            var reportData = reportService.GetWinLoss(winLossParam);

            var viewModel = new WinLossViewModel(
                reportData,
                message,
                winLossParam,
                BuildBreadcrumbLevels(message),
                BuildProductSelectListItems(winLossParam.UserSelectedProductIds, products));

            return viewModel;
        }
    }
}