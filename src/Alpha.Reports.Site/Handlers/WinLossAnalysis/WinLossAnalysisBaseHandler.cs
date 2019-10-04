namespace Alpha.Reports.Site.Handlers.WinLossAnalysis
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Extensions;
    using Params;
    using Reports.Models;
    using Reports.Models.WinLossAnalysis;
    using Requests.WinLossAnalysis;
    using Services;

    [SuppressMessage(
         "StyleCop.CSharp.NamingRules",
         "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
         Justification = "Reviewed. Suppression is OK here.")]
    public class WinLossAnalysisBaseHandler : BaseHandler
    {
        protected readonly ProductService productService;
        protected readonly WinLossAnalysisService reportService;

        public WinLossAnalysisBaseHandler(
            ProductService productService,
            WinLossAnalysisService reportService)
        {
            this.productService = productService;
            this.reportService = reportService;
        }

        protected WinLossAnalysisParam CreateWinLossAnalysisParamater(WinLossAnalysisQuery message, IEnumerable<Product> excludeProducts = null)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(
                message.CustId,
                message.UserSelectedProductIds.ToListInt(),
                message.MinDate,
                excludeProducts);

            return new WinLossAnalysisParam(
                 message.CustId,
                 message.ReportTypeId,
                 message.FromDate,
                 message.ToDate,
                 message.LimitedDateToShowInfo,
                 validUserSelectedProductIds,
                 message.CustLevel);
        }

        internal WinLossAnalysisModel AccumulateGrandTotal(IEnumerable<WinLossAnalysisModel> reportData)
        {
            var grandTotal = new WinLossAnalysisModel();

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