namespace Alpha.Reports.Site.Handlers.WinLossByProduct
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using App_Start;
    using Params;
    using RemotingRef;
    using Reports.Models.WinlossByBetType;
    using Reports.Models.WinLossByProduct;
    using Requests.WinLossByBetType;
    using Requests.WinLossByProduct;
    using Services;
    using Sunplus.Agent.Model.BL.Definitions;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class WinLossByProductBaseHandler : BaseHandler
    {
        protected readonly WinLossByProductService reportService;
        protected readonly CacheRefService cacheRefService;

        public WinLossByProductBaseHandler(WinLossByProductService reportService, CacheRefService cacheRefService)
        {
            this.reportService = reportService;
            this.cacheRefService = cacheRefService;
        }

        protected WinLossByProductParam CreateWinLossByProductParamater(WinLossByProductQuery message)
        {
            return new WinLossByProductParam(
                 message.CustId,
                 message.FromDate,
                 message.ToDate,
                 message.CustLevel);
        }

        internal WinLossByProductModel AccumulateGrandTotal(IEnumerable<WinLossByProductModel> reportData)
        {
            var grandTotal = new WinLossByProductModel();

            if (reportData != null && reportData.Any())
            {
                foreach (var data in reportData)
                {
                    grandTotal.Add(data);
                }
            }

            return grandTotal;
        }

        [SuppressMessage(
            "StyleCop.CSharp.StylecopPlus.Rules", 
            "SP2101:MethodMustNotContainMoreLinesThan", 
            Justification = "Reviewed. Suppression is OK here.")]
        public string BuildDisplayedProdutName(int productId, BetType betTypeId, string betTypeName, bool liveIndicator)
        {
            if (BetTypeConfig.DisplayWinLossByBetTypeRules.ContainsKey(productId))
            {
                return BetTypeConfig.DisplayWinLossByBetTypeRules[productId](betTypeId, betTypeName, liveIndicator, cacheRefService);
            }

            return betTypeName;
        }
    }
}