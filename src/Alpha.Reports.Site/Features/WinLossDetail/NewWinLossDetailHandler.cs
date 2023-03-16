namespace Alpha.Reports.Site.Features.WinLossDetail
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Reports.Models;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Features.WinLossDetail.Requests;
    using Alpha.Reports.Site.Features.WinLossDetail.ViewModels;
    using Alpha.Reports.Site.ViewModels.Components;
    using MediatR;

    public class NewWinLossDetailHandler : IAsyncRequestHandler<NewWinLossDetailQuery, NewWinLossDetailViewModel>
    {
        private readonly ProductService productService;
        private readonly ProductDetailService productDetailService;
        private readonly WinLossService winlossService;
        private readonly WinLossDetailService winlossDetailService;

        public NewWinLossDetailHandler(
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

        public async Task<NewWinLossDetailViewModel> Handle(NewWinLossDetailQuery message)
        {
            var productsTask = productService.GetProductsForCustomerAsync(message.ViewerId, message.Site, message.CurrencyId, message.MinDate, message.IsEnableSabaPromotion);
            var productDetailsTask = productDetailService.GetProductDetailsAsync(message.ViewerId, message.Site, message.CurrencyId);
            await Task.WhenAll(productsTask, productDetailsTask);

            var products = await productsTask;
            var productDetails = await productDetailsTask;
            //var winLossDetailParam = new WinLossDetailParam(
            //     message.CustId,
            //     message.ViewerId,
            //     message.Site,
            //     message.CurrencyId,
            //     message.PageIndex,
            //     message.PageSize,
            //     message.FromDate,
            //     message.ToDate,
            //     message.LimitedDateToShowInfo,
            //     validUserSelectedProductIds,
            //     validUserSelectedResourceIds,
            //     userLevel,
            //     message.SortPropertyName,
            //     message.IsDescendingSort,
            //     message.IsHistoricReport,
            //     productDetailService.BuildProductDetailsXML(validUserSelectedProductIds, validUserSelectedResourceIds),
            //     message.IsEnableSabaPromotion);

            //var reportDataTask = winlossDetailService.GetWinLossDetailAsync(winLossDetailParam);
            var reportDataTask = Task.FromResult(0);
            var totalEnteredSportTask = winlossService.GetTotalEnteredSportYesterdayAndTodayAsync();
            await Task.WhenAll(reportDataTask, totalEnteredSportTask);

            var viewModel = new NewWinLossDetailViewModel(
                message,
                //await reportDataTask,
                BuildGroupProductViewModels(products),
                BuildProductDetailViewModels(productDetails),
                await totalEnteredSportTask);

            return viewModel;
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
    }
}