namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System.Threading.Tasks;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Requests.WinLossDetail;
    using Alpha.Reports.Site.ViewModels.WinLossDetail;
    using MediatR;

    public class WinLossDetailNewHandler : WinLossDetailBaseNewHandler,
        IAsyncRequestHandler<WinLossDetailNewQuery, WinLossDetailNewViewModel>,
        IAsyncRequestHandler<WinLossDetailGetDataQuery, WinLossDetailGetDataViewModel>
    {
        private readonly WinLossService winlossService;

        public WinLossDetailNewHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossService winlossService,
            WinLossDetailService winlossDetailService) : base(productService, productDetailService, winlossDetailService)
        {
            this.winlossService = winlossService;
        }

        public async Task<WinLossDetailNewViewModel> Handle(WinLossDetailNewQuery message)
        {
            var productsAndProductDetails = await GetProductsAndProductDetails(message);
            var products = productsAndProductDetails.Item1;
            var productDetails = productsAndProductDetails.Item2;

            var winLossDetailParam = CreateParameter(message, products, productDetails);
            var reportDataTask = winlossDetailService.GetWinLossDetailAsync(winLossDetailParam);
            var totalEnteredSportTask = winlossService.GetTotalEnteredSportYesterdayAndTodayAsync();
            await Task.WhenAll(reportDataTask, totalEnteredSportTask);

            return new WinLossDetailNewViewModel(
                message,
                BuildProductViewModels(message, products),
                BuildProductDetailNewViewModels(message, productDetails),
                await totalEnteredSportTask,
                await reportDataTask);
        }

        public async Task<WinLossDetailGetDataViewModel> Handle(WinLossDetailGetDataQuery message)
        {
            var winLossDetailParam = await CreateParameter(message);
            var reportDataTask = winlossDetailService.GetWinLossDetailAsync(winLossDetailParam);
            var totalEnteredSportTask = winlossService.GetTotalEnteredSportYesterdayAndTodayAsync();
            await Task.WhenAll(reportDataTask, totalEnteredSportTask);

            return new WinLossDetailGetDataViewModel(
                message.CustLevel,
                !message.IsProtectedMode,
                await totalEnteredSportTask,
                await reportDataTask);
        }
    }
}