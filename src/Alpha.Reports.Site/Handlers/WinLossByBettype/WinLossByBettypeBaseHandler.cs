namespace Alpha.Reports.Site.Handlers.WinLossByBetType
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Params;
    using Reports.Models.WinlossByBettype;
    using Requests.WinLossByBettype;
    using Services;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class WinLossByBetTypeBaseHandler : BaseHandler
    {
        protected readonly WinLossByBetTypeService reportService;

        public WinLossByBetTypeBaseHandler(WinLossByBetTypeService reportService)
        {
            this.reportService = reportService;
        }

        protected WinLossByBetTypeParam CreateWinLossByBettypeParamater(WinLossByBettypeQuery message)
        {
            return new WinLossByBetTypeParam(
                 message.CustId,
                 message.FromDate,
                 message.ToDate,
                 message.CustLevel);
        }

        internal WinLossByBetTypeModel AccumulateGrandTotal(IEnumerable<WinLossByBetTypeModel> reportData)
        {
            var grandTotal = new WinLossByBetTypeModel();
           
            if (reportData != null && reportData.Any())
            {
                var reportDataCopy = reportData.ToList();
                int index, count;
                for (index = 1, count = reportDataCopy.Count, grandTotal = reportDataCopy[0]; index < count; index++)
                {
                    grandTotal = WinLossByBetTypeModel.Add(grandTotal, reportDataCopy[index]);
                }
            }

            return grandTotal;
        }
    }
}