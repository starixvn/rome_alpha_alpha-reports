namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Alpha.Reports.Constants;
    using Extensions;
    using Params;
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
        protected readonly ProductDetailService productDetailService;
        protected readonly WinLossService reportService;

        public WinLossBaseHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossService reportService)
        {
            this.productService = productService;
            this.productDetailService = productDetailService;
            this.reportService = reportService;
        }

        protected WinLossParam CreateWinLossParamater(WinLossQuery message)
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

            return new WinLossParam(
                 message.CustId,
                 message.Site,
                 message.CurrencyId,
                 message.FromDate,
                 message.ToDate,
                 message.LimitedDateToShowInfo,
                 validUserSelectedProductIds,
                 validUserSelectedResourceIds,
                 message.CustLevel,
                 message.IsHistoricReport,
                 productDetailService.BuildProductDetailsXML(message.CustId, validUserSelectedProductIds, validUserSelectedResourceIds));
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