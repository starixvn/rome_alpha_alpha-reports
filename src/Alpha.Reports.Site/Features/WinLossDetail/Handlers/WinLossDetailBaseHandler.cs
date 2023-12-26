namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System.Diagnostics.CodeAnalysis;
    using Alpha.Reports.Constants;
    using Alpha.Reports.Site.Features.EnteredResult;
    using Extensions;
    using Params;
    using Reports.Models;
    using Requests.WinLossDetail;
    using Services;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class WinLossDetailBaseHandler : EnteredSportResultBaseHandler
    {
        protected readonly ProductService productService;
        protected readonly ProductDetailService productDetailService;
        protected readonly WinLossDetailService winlossDetailService;

        public WinLossDetailBaseHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossDetailService winlossDetailService,
            WinLossService winLossService) : base(winLossService)
        {
            this.productService = productService;
            this.productDetailService = productDetailService;
            this.winlossDetailService = winlossDetailService;
        }

        protected WinLossDetailParam CreateParameter(
            WinLossDetailQuery message,
            UserLevel userLevel)
        {
            var validUserSelectedProductIds = productService.FilterProductIds(
                message.Site,
                message.CurrencyId,
                message.UserSelectedProductIds.ToListInt(),
                message.MinDate);
            var validUserSelectedResourceIds = productDetailService.FilterResourceIdsByCustomer(
                message.ViewerId,
                message.Site,
                message.CurrencyId,
                !message.IsPostBack ? DefaultVariables.AllProductDetails : message.UserSelectedResourceIds);
            validUserSelectedProductIds = productDetailService.RemoveProductIds(validUserSelectedProductIds, validUserSelectedResourceIds);

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
                 productDetailService.BuildProductDetailsXML(validUserSelectedProductIds, validUserSelectedResourceIds));
        }
    }
}