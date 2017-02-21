namespace Alpha.Reports.Site.Handlers.WinLossByBetType
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using App_Start;
    using Params;
    using RemotingRef;
    using Reports.Models.WinlossByBetType;
    using Requests.WinLossByBetType;
    using Services;
    using Sunplus.Agent.Model.BL.Definitions;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class WinLossByBetTypeBaseHandler : BaseHandler
    {
        protected readonly WinLossByBetTypeService reportService;
        protected readonly CacheRefService cacheRefService;

        public WinLossByBetTypeBaseHandler(WinLossByBetTypeService reportService, CacheRefService cacheRefService)
        {
            this.reportService = reportService;
            this.cacheRefService = cacheRefService;
        }

        protected WinLossByBetTypeParam CreateWinLossByBettypeParamater(WinLossByBetTypeQuery message)
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
        public string BuildDisplayedBetTypeName(int productId, BetType betTypeId, string betTypeName, bool liveIndicator)
        {
            if (BetTypeConfig.DisplayWinLossByBetTypeRules.ContainsKey(productId))
            {
                return BetTypeConfig.DisplayWinLossByBetTypeRules[productId](betTypeId, betTypeName, liveIndicator, cacheRefService);
            }

            return betTypeName;
        }
    }
}