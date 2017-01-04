namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System.Diagnostics.CodeAnalysis;
    using Extensions;
    using Params;
    using Reports.Models;
    using Requests.WinLoss;
    using Services;
    using Services.WinLoss;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class WinLossBaseHandler : BaseHandler
    {
        protected readonly ProductService productService;

        protected readonly WinLossService reportService;

        public WinLossBaseHandler(
            ProductService productService,
            WinLossService reportService)
        {
            this.productService = productService;
            this.reportService = reportService;
        }

        protected WinLossParam CreateWinLossParamater(
             WinLossQuery message,
             UserLevel userLevel)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(
                message.CustId,
                message.UserSelectedProductIds.ToListInt());

            return new WinLossParam(
                 message.CustId,
                 message.FromDate,
                 message.ToDate,
                 validUserSelectedProductIds,
                 userLevel,
                 message.IsHistoricReport);
        }
    }
}