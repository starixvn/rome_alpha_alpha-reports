namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.Reports.Configurations;
    using Alpha.Reports.Constants;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Requests.WinLossDetail;
    using Alpha.Reports.Site.ViewModels.Components;
    using Alpha.Reports.Site.ViewModels.WinLossDetail;
    using MediatR;

    public class WinLossDetailNewHandler : WinLossDetailBaseNewHandler,
        IAsyncRequestHandler<WinLossDetailNewQuery, WinLossDetailNewViewModel>,
        //IAsyncRequestHandler<WinLossDetailGetDataQuery, WinLossDetailGetDataViewModel>,
        IAsyncRequestHandler<ReportLegendNewQuery, ReportLegendViewModel>
    {
        private readonly WinLossService winlossService;
        private readonly WinLossDetailService winlossDetailService;

        public WinLossDetailNewHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossService winlossService,
            WinLossDetailService winlossDetailService) : base(productService, productDetailService)
        {
            this.winlossService = winlossService;
            this.winlossDetailService = winlossDetailService;
        }

        public async Task<WinLossDetailNewViewModel> Handle(WinLossDetailNewQuery message)
        {
            var productsTask = productService.GetProductsForCustomerAsync(message.ViewerId, message.Site, message.CurrencyId, message.MinDate, message.IsEnableSabaPromotion);
            var productDetailsTask = productDetailService.GetProductDetailsAsync(message.ViewerId, message.Site, message.CurrencyId);
            var totalEnteredSportTask = winlossService.GetTotalEnteredSportYesterdayAndTodayAsync();
            await Task.WhenAll(productsTask, productDetailsTask, totalEnteredSportTask);
            var products = (await productsTask).ToList();
            var productDetails = (await productDetailsTask).ToList();

            var winLossDetailParam = CreateParameter(message, products, productDetails);
            var reportData = await winlossDetailService.GetWinLossDetailAsync(winLossDetailParam);

            return new WinLossDetailNewViewModel(
                message,
                BuildProductViewModels(message.ProductIds, products),
                BuildProductDetailNewViewModels(message.ResourceIds, productDetails),
                await totalEnteredSportTask,
                reportData);
        }

        //public async Task<WinLossDetailGetDataViewModel> Handle(WinLossDetailGetDataQuery message)
        //{
        //    var winLossDetailParam = await CreateParameter(message, message.CustLevel);
        //    var reportDataTask = winlossDetailService.GetWinLossDetailAsync(winLossDetailParam);
        //    var totalEnteredSportTask = winlossService.GetTotalEnteredSportYesterdayAndTodayAsync();
        //    await Task.WhenAll(reportDataTask, totalEnteredSportTask);

        //    return new WinLossDetailGetDataViewModel(await reportDataTask, await totalEnteredSportTask);
        //}

        public async Task<ReportLegendViewModel> Handle(ReportLegendNewQuery message)
            => new ReportLegendViewModel(await GetReportLegend(message), GetGrossCommNotes(message.ViewerLevel));

        public async Task<IEnumerable<string>> GetReportLegend(ReportLegendNewQuery message)
        {
            var reportLegend = new List<string>();

            if (message.IsSupportHistoricReport && !message.IsHistoricReport)
            {
                reportLegend.Add(string.Format(
                    $"- {Resources.CanOnlyViewDataAfterTimeCurrentReport}", message.MinDate.ToShortDateWithSlash()));
            }

            if (message.IsSupportHistoricReport)
            {
                reportLegend.Add(string.Format(
                    $"- {string.Format(Resources.SabaSportsNote, SiteSettings.SeperateSabaSportsDate)}"));
            }

            reportLegend.Add(Resources.Tooltip_NetTurnover);

            var liveCasinoTooltipsTask = BuildTooltipsForLiveCasino(message.ViewerId, message.Site, message.CurrencyId);
            var rngCasinoTooltipsTask = BuildTooltipsForRNGCasino(message.ViewerId, message.Site, message.CurrencyId);
            var productsNeedDelayTimeToSumarizeDataTask = productService
                .GetProductsNeedDelayTimeToSumarizeDataByCustomerAsync(message.ViewerId, message.Site, message.CurrencyId);
            await Task.WhenAll(liveCasinoTooltipsTask, rngCasinoTooltipsTask, productsNeedDelayTimeToSumarizeDataTask);

            var liveCasinoTooltips = await liveCasinoTooltipsTask;
            if (!string.IsNullOrEmpty(liveCasinoTooltips))
            {
                reportLegend.Add(liveCasinoTooltips);
            }

            var rngCasinoTooltips = await rngCasinoTooltipsTask;
            if (!string.IsNullOrEmpty(rngCasinoTooltips))
            {
                reportLegend.Add(rngCasinoTooltips);
            }

            var productsNeedDelayTimeToSumarizeData = (await productsNeedDelayTimeToSumarizeDataTask)
                .Select(product => (int)product)
                .ToList();

            if (productsNeedDelayTimeToSumarizeData.Any())
            {
                reportLegend.Add(string.Format(
                    Resources.DelayIssueLegend,
                    string.Join(", ", productService.GetProductNames(productsNeedDelayTimeToSumarizeData))));
            }

            return reportLegend;
        }

        private async Task<string> BuildTooltipsForLiveCasino(int viewerId, string site, int currencyId)
        {
            var offeredLiveCasinoNames = (await productService.FilterProductsByCustomerAsync(
                viewerId,
                site,
                currencyId,
                ProductIdConstants.ProductListForTooltipsLiveCasino)).Select(i => i.Name).ToList();

            return offeredLiveCasinoNames.Any()
                ? string.Format(Resources.Tooltip_NetTurnover_livecasino, string.Join(", ", offeredLiveCasinoNames))
                : string.Empty;
        }

        private async Task<string> BuildTooltipsForRNGCasino(int viewerId, string site, int currencyId)
        {
            var offeredRNGCasinoNames = (await productService.GetProductsForCustomerAsync(viewerId, site, currencyId))
                .Where(i => i.Type == ProductIdConstants.DummyRNGProductId || ProductIdConstants.ProductListForTooltipsRNGCasino.Contains(i.Id))
                .Select(i => i.DisplayName)
                .ToList();

            return offeredRNGCasinoNames.Any()
                ? string.Format(Resources.Tooltip_NetTurnover_RNGCasino, string.Join(", ", offeredRNGCasinoNames))
                : string.Empty;
        }
    }
}