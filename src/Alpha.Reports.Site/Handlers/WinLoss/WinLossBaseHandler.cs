namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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

        protected readonly WinLossService reportService;

        public WinLossBaseHandler(
            ProductService productService,
            WinLossService reportService)
        {
            this.productService = productService;
            this.reportService = reportService;
        }

        protected WinLossParam CreateWinLossParamater(WinLossQuery message)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(
                message.CustId,
                message.UserSelectedProductIds.ToListInt());

            return new WinLossParam(
                 message.CustId,
                 message.FromDate,
                 message.ToDate,
                 validUserSelectedProductIds,
                 message.CustLevel,
                 message.IsHistoricReport);
        }

        internal virtual WinLossModel AccumulateGrantTotal(IEnumerable<WinLossModel> reportData)
        {
            var grantTotal = new WinLossModel();
           
            if (reportData != null && reportData.Any())
            {
                var reportDataCopy = reportData.ToList();
                int index, count;
                for (index = 1, count = reportDataCopy.Count, grantTotal = reportDataCopy[0]; index < count; index++)
                {
                    grantTotal = WinLossModel.Add(grantTotal, reportDataCopy[index]);
                }
            }

            return grantTotal;
        }
    }
}