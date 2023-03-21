namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models;
    using Alpha.Reports.Params;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Requests.WinLossDetail;
    using Alpha.Reports.Site.ViewModels.Components;
    using Alpha.Reports.Site.ViewModels.WinLossDetail;
    using MediatR;

    public class WinLossDetailHandlerNew :
        IAsyncRequestHandler<WinLossDetailIndexQuery, WinLossDetailIndexViewModel>,
        IAsyncRequestHandler<WinLossDetailQueryNew, WinLossDetailViewModelNew>
    {
        private readonly ProductService productService;
        private readonly ProductDetailService productDetailService;
        private readonly WinLossService winlossService;
        private readonly WinLossDetailService winlossDetailService;

        public WinLossDetailHandlerNew(
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossService winlossService,
            WinLossDetailService winlossDetailService)
        {
            this.productService = productService;
            this.productDetailService = productDetailService;
            this.winlossService = winlossService;
            this.winlossDetailService = winlossDetailService;
        }

        public async Task<WinLossDetailIndexViewModel> Handle(WinLossDetailIndexQuery message)
        {
            var productsTask = productService.GetProductsForCustomerAsync(message.ViewerId, message.Site, message.CurrencyId, message.MinDate, message.IsEnableSabaPromotion);
            var productDetailsTask = productDetailService.GetProductDetailsAsync(message.ViewerId, message.Site, message.CurrencyId);
            var totalEnteredSportTask = winlossService.GetTotalEnteredSportYesterdayAndTodayAsync();
            await Task.WhenAll(productsTask, productDetailsTask, totalEnteredSportTask);

            return new WinLossDetailIndexViewModel(
                message,
                BuildGroupProductViewModels(await productsTask),
                BuildProductDetailViewModels(await productDetailsTask),
                await totalEnteredSportTask);
        }

        public async Task<WinLossDetailViewModelNew> Handle(WinLossDetailQueryNew message)
        {
            var winLossDetailParam = await CreateParameter(message, message.CustLevel);
            var reportDataTask = winlossDetailService.GetWinLossDetailAsync(winLossDetailParam);
            var totalEnteredSportTask = winlossService.GetTotalEnteredSportYesterdayAndTodayAsync();
            await Task.WhenAll(reportDataTask, totalEnteredSportTask);

            return new WinLossDetailViewModelNew(
                message,
                await reportDataTask,
                await totalEnteredSportTask);
        }

        private static IEnumerable<GroupProductViewModel> BuildGroupProductViewModels(IEnumerable<Product> products)
            => products?.Any() == true
                ? products
                    .GroupBy(product => new { product.Type, product.TypeName })
                    .Select(grp => new GroupProductViewModel(
                        new SelectListGroup { Name = grp.Key.TypeName },
                        grp.Select(product => new SelectListItem
                        {
                            Text = product.DisplayName,
                            Value = product.Id.ToString(),
                            Selected = true
                        })))
                : Enumerable.Empty<GroupProductViewModel>();

        private static IEnumerable<ProductDetailViewModel> BuildProductDetailViewModels(IEnumerable<ProductDetail> productDetails)
            => productDetails?.Any() == true
                ? productDetails.Select(p => new ProductDetailViewModel(p.ProductId, p.ResourceId, p.DisplayName, true))
                : Enumerable.Empty<ProductDetailViewModel>();

        private async Task<WinLossDetailParam> CreateParameter(WinLossDetailQueryNew message, UserLevel userLevel)
        {
            var validUserSelectedProductIdsTask = productService.FilterProductIdsByCustomerAsync(
                message.ViewerId,
                message.Site,
                message.CurrencyId,
                message.UserSelectedProductIds.ToListInt(),
                message.MinDate,
                message.IsEnableSabaPromotion);
            var validUserSelectedResourceIdsTask = productDetailService.FilterResourceIdsByCustomerAsync(
                message.ViewerId,
                message.Site,
                message.CurrencyId,
                message.UserSelectedResourceIds);
            await Task.WhenAll(validUserSelectedProductIdsTask, validUserSelectedResourceIdsTask);
            var validUserSelectedProductIds = await validUserSelectedProductIdsTask;
            var validUserSelectedResourceIds = await validUserSelectedResourceIdsTask;
            validUserSelectedProductIds = await productDetailService.RemoveProductIdsAsync(validUserSelectedProductIds, validUserSelectedResourceIds);

            return new WinLossDetailParam(
                message.CustId,
                message.ViewerId,
                message.Site,
                message.CurrencyId,
                message.PageIndex,
                message.PageSize,
                message.FromDate,
                message.ToDate,
                message.LimitedDateToShowInfo,
                validUserSelectedProductIds,
                validUserSelectedResourceIds,
                userLevel,
                message.SortPropertyName,
                message.IsDescendingSort,
                message.IsHistoricReport,
                await productDetailService.BuildProductDetailsXMLAsync(validUserSelectedProductIds, validUserSelectedResourceIds),
                message.IsEnableSabaPromotion);
        }
    }
}