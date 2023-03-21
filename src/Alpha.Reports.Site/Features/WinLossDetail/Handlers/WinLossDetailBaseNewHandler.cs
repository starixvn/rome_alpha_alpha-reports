namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System.Threading.Tasks;
    using Alpha.Reports.Extensions;
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

        protected async Task<WinLossDetailParam> CreateParameter(WinLossDetailNewQuery message, UserLevel userLevel)
        {
            var validUserSelectedProductIdsTask = productService.FilterProductIdsByCustomerAsync(
                message.ViewerId,
                message.Site,
                message.CurrencyId,
                message.ProductIds.ToListInt(),
                message.MinDate,
                message.IsEnableSabaPromotion);
            var validUserSelectedResourceIdsTask = productDetailService.FilterResourceIdsByCustomerAsync(
                message.ViewerId,
                message.Site,
                message.CurrencyId,
                message.ResourceIds);
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
                null,
                false,
                message.IsHistoricReport,
                await productDetailService.BuildProductDetailsXMLAsync(validUserSelectedProductIds, validUserSelectedResourceIds),
                message.IsEnableSabaPromotion);
        }
    }
}