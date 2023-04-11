namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System.Collections.Generic;
    using Alpha.Reports.Models;
    using Alpha.Reports.Params;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Handlers;
    using Alpha.Reports.Site.Requests.WinLossDetail;

    public class WinLossDetailBaseNewHandler : BaseHandler
    {
        protected readonly ProductService productService;
        protected readonly ProductDetailService productDetailService;

        protected WinLossDetailBaseNewHandler(
            ProductService productService,
            ProductDetailService productDetailService)
        {
            this.productService = productService;
            this.productDetailService = productDetailService;
        }

        protected WinLossDetailParam CreateParameter(
            WinLossDetailNewQuery message, 
            IEnumerable<Product> products,
            IEnumerable<ProductDetail> productDetails)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(products, message.ProductIds);
            var validUserSelectedResourceIds = productDetailService.FilterResourceIdsByCustomer(productDetails, message.ResourceIds);
            validUserSelectedProductIds = productDetailService.RemoveProductIds(validUserSelectedProductIds, validUserSelectedResourceIds, productDetails);

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
                message.CustLevel,
                null,
                false,
                message.IsHistoricReport,
                productDetailService.BuildProductDetailsXML(validUserSelectedProductIds, validUserSelectedResourceIds),
                message.IsEnableSabaPromotion);
        }
    }
}