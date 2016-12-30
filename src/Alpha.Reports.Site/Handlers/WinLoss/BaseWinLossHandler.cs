namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Extensions;
    using Params;
    using Reports.Models;
    using Requests.WinLoss;
    using Services;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class BaseWinLossHandler : BaseHandler
    {
        protected readonly ProductService productService;

        protected readonly WinLossService reportService;

        public BaseWinLossHandler(
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
                string.IsNullOrWhiteSpace(message.UserSelectedProductIds) ? Enumerable.Empty<int>() : message.UserSelectedProductIds.ToListInt());

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