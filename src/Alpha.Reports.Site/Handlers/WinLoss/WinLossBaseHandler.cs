namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Extensions;
    using Params;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Requests.WinLoss;
    using Services;

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

        protected WinLossParam CreateWinLossParamater(WinLossQuery message, IEnumerable<Product> excludeProducts = null)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(
                message.CustId,
                message.UserSelectedProductIds.ToListInt(),
                message.MinDate,
                excludeProducts);

            return new WinLossParam(
                 message.CustId,
                 message.FromDate,
                 message.ToDate,
                 message.LimitedDateToShowInfo,
                 validUserSelectedProductIds,
                 message.CustLevel,
                 message.IsHistoricReport);
        }

        internal WinLossModel AccumulateGrandTotal(IEnumerable<WinLossModel> reportData)
        {
            var grandTotal = new WinLossModel();

            if (reportData != null && reportData.Any())
            {
                foreach (var data in reportData)
                {
                    grandTotal.Add(data);
                }
            }

            return grandTotal;
        }
    }
}