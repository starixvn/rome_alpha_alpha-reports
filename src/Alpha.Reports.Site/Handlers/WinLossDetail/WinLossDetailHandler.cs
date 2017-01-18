namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System;
    using MediatR;
    using Requests.WinLossDetail;
    using Services;
    using ViewModels.WinLossDetail;

    public class WinLossDetailHandler :
        WinLossDetailBaseHandler,
        IRequestHandler<WinLossDetailQuery, WinLossDetailViewModel>
    {
        public WinLossDetailHandler(
            ProductService productService,
            WinLossDetailService reportService) : base(productService, reportService)
        {
        }

        public WinLossDetailViewModel Handle(WinLossDetailQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var products = productService.GetProductsForCustomer(message.CustId);

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
    }
}