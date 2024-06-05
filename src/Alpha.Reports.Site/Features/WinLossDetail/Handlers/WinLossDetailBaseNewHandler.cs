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
        private readonly CustomerService customerService;

        protected WinLossDetailBaseNewHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossDetailService winlossDetailService,
            CustomerService customerService)
        {
            this.productService = productService;
            this.productDetailService = productDetailService;
            this.winlossDetailService = winlossDetailService;
            this.customerService = customerService;
        }

        protected async Task<Tuple<IEnumerable<Product>, IEnumerable<ProductDetail>>> GetProductsAndProductDetails(
            WinLossDetailGetDataWeekQuery message)
        {
            var productsTask = productService.GetAllOfferedProductsAsync(message.Site, message.CurrencyId, message.MinDate);
            var productDetailsTask = productDetailService.GetProductDetailsAsync(message.ViewerId, message.Site, message.CurrencyId);
            await Task.WhenAll(productsTask, productDetailsTask);

            IEnumerable<Product> products = (await productsTask).ToList();
            IEnumerable<ProductDetail> productDetails = (await productDetailsTask).ToList();
            return Tuple.Create(products, productDetails);
        }

        protected async Task<WinLossDetailParam> CreateParameter(WinLossDetailGetDataWeekQuery message)
        {
            var productsAndProductDetails = await GetProductsAndProductDetails(message);

            return CreateParameter(message, productsAndProductDetails.Item1, productsAndProductDetails.Item2);
        }

        protected WinLossDetailParam CreateParameter(
            WinLossDetailGetDataWeekQuery message,
            IEnumerable<Product> products,
            IEnumerable<ProductDetail> productDetails)
        {
            var validUserSelectedProductIds = GetValidUserSelectedProductIds(message, products);
            var validUserSelectedResourceIds = GetValidUserSelectedResourceIds(message, productDetails);

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
                productDetailService.BuildProductDetailsXML(validUserSelectedResourceIds),
                message.Username);
        }

        private static IEnumerable<int> GetValidUserSelectedProductIds(
            WinLossDetailGetDataWeekQuery message,
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
            WinLossDetailGetDataWeekQuery message,
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
            WinLossDetailGetDataWeekQuery message,
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
                            productList.AddRange(item.Select(p => new ProductViewModel(
                                p.Id,
                                p.DisplayName,
                                isSelectAll || userSelectedProductIds.ContainsKey(p.Id),
                                null)));
                        }
                        else
                        {
                            var children = item.Select(p => new ProductViewModel(
                                p.Id,
                                p.DisplayName,
                                isSelectAll || userSelectedProductIds.ContainsKey(p.Id),
                                null)).ToList();
                            var isGroupProductChecked = children.All(p => p.Checked);
                            productList.Add(new ProductViewModel(
                                item.Key.Type,
                                item.Key.TypeName,
                                isGroupProductChecked,
                                children));
                        }
                    });
            }

            return productList;
        }

        protected static IEnumerable<ProductDetailNewViewModel> BuildProductDetailNewViewModels(
            WinLossDetailGetDataWeekQuery message,
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

        protected bool BuildWinLossDetailGetDataQuery(WinLossDetailGetDataWeekQuery message)
        {
            var isValidUsername = true;

            if (!string.IsNullOrWhiteSpace(message.Username))
            {
                var customerLevel = customerService.GetCustomerLevel(message.Username, message.ViewerId);
                isValidUsername = customerLevel != null;

                if (isValidUsername)
                {
                    var custInfo = customerService.GetCustInfo(customerLevel);

                    var nickname = message.ViewerLevel.IsDirectUplineOf(customerLevel.RoleId)
                        ? customerLevel.Username2
                        : string.Empty;
                    customerService.UpdateUserSearch(
                        message.ViewerId,
                        customerLevel.Username,
                        customerLevel.FirstName,
                        customerLevel.LastName,
                        nickname);

                    message.CustId = custInfo.CustId;
                    message.CustName = custInfo.CustName;
                    message.CustLevelId = custInfo.CustLevelId;

                    message.BreadcrumbLevel = customerService.GetCustInfoList(customerLevel, message.ViewerLevel).ToList();
                }
            }

            if (!message.BreadcrumbLevel.Any())
            {
                message.BreadcrumbLevel = new[]
                {
                    new CustInfo(message.ViewerId, message.CustName, message.ViewerLevelId)
                };
            }

            return isValidUsername;
        }
    }
}