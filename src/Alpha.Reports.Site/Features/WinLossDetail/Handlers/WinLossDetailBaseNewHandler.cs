namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.Reports.Models;
    using Alpha.Reports.Params;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Features.Shared.ViewModels.Components;
    using Alpha.Reports.Site.Handlers;
    using Alpha.Reports.Site.Requests.WinLossDetail;

    public class WinLossDetailBaseNewHandler : BaseHandler
    {
        protected readonly ProductService productService;
        protected readonly ProductDetailService productDetailService;
        protected readonly WinLossDetailService winlossDetailService;

        protected WinLossDetailBaseNewHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossDetailService winlossDetailService)
        {
            this.productService = productService;
            this.productDetailService = productDetailService;
            this.winlossDetailService = winlossDetailService;
        }

        protected async Task<Tuple<IEnumerable<Product>, IEnumerable<ProductDetail>>> GetProductsAndProductDetails(
            WinLossDetailGetDataQuery message)
        {
            var productsTask = productService.GetProductsForCustomerAsync(message.ViewerId, message.Site, message.CurrencyId, message.MinDate, message.IsEnableSabaPromotion);
            var productDetailsTask = productDetailService.GetProductDetailsAsync(message.ViewerId, message.Site, message.CurrencyId);
            await Task.WhenAll(productsTask, productDetailsTask);

            IEnumerable<Product> products = (await productsTask).ToList();
            IEnumerable<ProductDetail> productDetails = (await productDetailsTask).ToList();
            return Tuple.Create(products, productDetails);
        }

        protected async Task<WinLossDetailParam> CreateParameter(WinLossDetailGetDataQuery message)
        {
            var productsAndProductDetails = await GetProductsAndProductDetails(message);

            return CreateParameter(message, productsAndProductDetails.Item1, productsAndProductDetails.Item2);
        }

        protected WinLossDetailParam CreateParameter(
            WinLossDetailGetDataQuery message,
            IEnumerable<Product> products,
            IEnumerable<ProductDetail> productDetails)
        {
            var validUserSelectedProductIds = GetValidUserSelectedProductIds(message, products);
            var validUserSelectedResourceIds = GetValidUserSelectedResourceIds(message, productDetails);
            validUserSelectedProductIds = productDetailService.RemoveProductIds(validUserSelectedProductIds, validUserSelectedResourceIds, productDetails);

            return new WinLossDetailParam(
                message.CustId,
                message.ViewerId,
                message.Site,
                message.CurrencyId,
                message.PageIndex,
                message.PageSize,
                message.WeekFromDate ?? message.FromDate,
                message.WeekToDate ?? message.ToDate,
                message.LimitedDateToShowInfo,
                validUserSelectedProductIds,
                validUserSelectedResourceIds,
                message.CustLevel,
                null,
                false,
                message.IsHistoricReport,
                productDetailService.BuildProductDetailsXML(validUserSelectedProductIds, validUserSelectedResourceIds),
                message.IsEnableSabaPromotion);
        }

        private static IEnumerable<int> GetValidUserSelectedProductIds(
            WinLossDetailGetDataQuery message,
            IEnumerable<Product> products)
        {
            var productIds = products.Select(product => product.Id);

            if (!message.IsSelectAllProducts)
            {
                var userSelectedProductIds = message.UserSelectedProductIds;

                productIds = productIds.Where(productId => userSelectedProductIds.ContainsKey(productId));
            }

            return productIds.ToList();
        }

        private static IEnumerable<int> GetValidUserSelectedResourceIds(
            WinLossDetailGetDataQuery message,
            IEnumerable<ProductDetail> productDetails)
        {
            var resourceIds = productDetails.Select(p => p.ResourceId);

            if (!message.IsSelectAllProductDetails)
            {
                var userSelectedResourceIds = message.UserSelectedResourceIds;

                resourceIds = resourceIds.Where(productId => userSelectedResourceIds.ContainsKey(productId));
            }

            return resourceIds.ToList();
        }

        protected static IEnumerable<ProductViewModel> BuildProductViewModels(
            WinLossDetailGetDataQuery message,
            IEnumerable<Product> products)
        {
            var productList = new List<ProductViewModel>();

            if (products?.Any() == true)
            {
                var isSelectAll = message.IsSelectAllProducts;
                var userSelectedProductIds = message.UserSelectedProductIds;
                products
                    .GroupBy(product => new { product.Type, product.TypeName })
                    .ToList()
                    .ForEach(item =>
                    {
                        if (item.Key.Type == 0)
                        {
                            productList.AddRange(item.Select(p =>
                                new ProductViewModel(p.Id, p.DisplayName, isSelectAll || userSelectedProductIds.ContainsKey(p.Id), null)));
                        }
                        else
                        {
                            var children = item.Select(
                                p => new ProductViewModel(p.Id, p.DisplayName, isSelectAll || userSelectedProductIds.ContainsKey(p.Id), null)).ToList();
                            productList.Add(new ProductViewModel(
                                item.Key.Type,
                                item.Key.TypeName,
                                children.All(p => p.Checked),
                                children));
                        }
                    });
            }

            return productList;
        }

        protected static IEnumerable<ProductDetailNewViewModel> BuildProductDetailNewViewModels(
            WinLossDetailGetDataQuery message,
            IEnumerable<ProductDetail> productDetails)
        {
            if (!productDetails.Any())
            {
                return Enumerable.Empty<ProductDetailNewViewModel>();
            }

            var isSelectAll = message.IsSelectAllProductDetails;
            var userSelectedResourceIds = message.UserSelectedResourceIds;

            return productDetails.Select(p => new ProductDetailNewViewModel(
                p.ProductId,
                p.ResourceId,
                p.DisplayName,
                isSelectAll || userSelectedResourceIds.ContainsKey(p.ResourceId)));
        }
    }
}