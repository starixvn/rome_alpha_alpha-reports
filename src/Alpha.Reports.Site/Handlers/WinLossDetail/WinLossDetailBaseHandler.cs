namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System.Diagnostics.CodeAnalysis;
    using Alpha.Reports.Constants;
    using Extensions;
    using Params;
    using Reports.Models;
    using Requests.WinLossDetail;
    using Services;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class WinLossDetailBaseHandler : BaseHandler
    {
        protected readonly ProductService productService;
        protected readonly ProductDetailService productDetailService;
        protected readonly WinLossDetailService reportService;

        public WinLossDetailBaseHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossDetailService reportService)
        {
            this.productService = productService;
            this.productDetailService = productDetailService;
            this.reportService = reportService;
        }

        protected WinLossDetailParam CreateParameter(
            WinLossDetailQuery message,
            UserLevel userLevel)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(
                message.CustId,
                message.Site,
                message.CurrencyId,
                message.UserSelectedProductIds.ToListInt(),
                message.MinDate);
            var validUserSelectedResourceIds = productDetailService.FilterResourceIdsByCustomer(
                message.CustId,
                !message.IsPostBack ? DefaultVariables.AllProductDetails : message.UserSelectedResourceIds);

            return new WinLossDetailParam(
                 message.CustId,
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
                 productDetailService.BuildProductDetailsXML(message.CustId, validUserSelectedProductIds, validUserSelectedResourceIds));
        }
    }
}