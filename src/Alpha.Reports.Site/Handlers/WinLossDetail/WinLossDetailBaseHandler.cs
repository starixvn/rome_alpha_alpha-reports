namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System.Diagnostics.CodeAnalysis;
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
        protected readonly WinLossDetailService reportService;

        public WinLossDetailBaseHandler(
            ProductService productService,
            WinLossDetailService reportService)
        {
            this.productService = productService;
            this.reportService = reportService;
        }

        protected WinLossDetailParam CreateParameter(
            WinLossDetailQuery message,
            UserLevel userLevel)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(
                message.CustId,
                message.UserSelectedProductIds.ToListInt(),
                message.MinDate);

            return new WinLossDetailParam(
                 message.CustId,
                 message.PageIndex,
                 message.PageSize,
                 message.FromDate,
                 message.ToDate,
                 validUserSelectedProductIds,
                 userLevel,
                 message.SortPropertyName,
                 message.IsDescendingSort,
                 message.IsHistoricReport);
        }
    }
}