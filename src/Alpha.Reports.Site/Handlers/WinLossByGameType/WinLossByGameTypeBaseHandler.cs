namespace Alpha.Reports.Site.Handlers.WinLossByGameType
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Params;
    using Reports.Models.WinlossByGameType;
    using Requests.WinLossByGameType;
    using Services;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class WinLossByGameTypeBaseHandler : BaseHandler
    {
        protected readonly WinLossByGameTypeService reportService;

        public WinLossByGameTypeBaseHandler(WinLossByGameTypeService reportService)
        {
            this.reportService = reportService;
        }

        protected WinLossByGameTypeParam CreateWinLossByGameTypeParamater(WinLossByGameTypeQuery message)
        {
            return new WinLossByGameTypeParam(
                 message.CustId,
                 message.ProductId,
                 message.FromDate,
                 message.ToDate,
                 message.CustLevel);
        }

        internal WinLossByGameTypeModel AccumulateGrandTotal(IEnumerable<WinLossByGameTypeModel> reportData)
        {
            var grandTotal = new WinLossByGameTypeModel();

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