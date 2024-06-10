namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Alpha.Reports.Constants;
    using Alpha.Reports.Site.Features.EnteredResult;
    using Extensions;
    using Params;
    using Reports.Models.Winloss;
    using Requests.WinLoss;
    using Services;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class WinLossBaseHandler : EnteredSportResultBaseHandler
    {
        protected readonly ProductService productService;
        protected readonly ProductDetailService productDetailService;

        public WinLossBaseHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossService reportService) : base(reportService)
        {
            this.productService = productService;
            this.productDetailService = productDetailService;
        }

        protected WinLossParam CreateWinLossParamater(WinLossQuery message)
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

            return new WinLossParam(
                 message.CustId,
                 message.ViewerId,
                 message.Site,
                 message.CurrencyId,
                 message.FromDate,
                 message.ToDate,
                 message.LimitedDateToShowInfo,
                 validUserSelectedProductIds,
                 validUserSelectedResourceIds,
                 message.CustLevel,
                 message.IsHistoricReport,
                 productDetailService.BuildProductDetailsXML(validUserSelectedResourceIds));
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