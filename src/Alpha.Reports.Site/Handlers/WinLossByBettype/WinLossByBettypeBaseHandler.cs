namespace Alpha.Reports.Site.Handlers.WinLossByBetType
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using App_Start;
    using Extensions;
    using Params;
    using RemotingRef;
    using Reports.Models.WinlossByBetType;
    using Reports.Models.WinLossByProduct;
    using Requests.WinLossByBetType;
    using Services;
    using Sunplus.Agent.Model.BL.Definitions;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class WinLossByBetTypeBaseHandler : BaseHandler
    {
        protected readonly ProductService productService;
        protected readonly WinLossByBetTypeService reportService;
        protected readonly CacheRefService cacheRefService;

        public WinLossByBetTypeBaseHandler(WinLossByBetTypeService reportService, ProductService productService, CacheRefService cacheRefService)
        {
            this.productService = productService;
            this.reportService = reportService;
            this.cacheRefService = cacheRefService;
        }

        protected WinLossByBetTypeParam CreateWinLossByBettypeParamater(WinLossByBetTypeQuery message)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(
                  message.CustId,
                  message.UserSelectedProductIds.ToListInt());

            return new WinLossByBetTypeParam(
                 message.CustId,
                 message.FromDate,
                 message.ToDate,
                 validUserSelectedProductIds,
                 message.CustLevel);
        }

        internal WinLossByBetTypeModel AccumulateGrandTotal(IEnumerable<WinLossByProductModel> reportData)
        {
            var grandTotal = new WinLossByBetTypeModel();

            if (reportData != null && reportData.Any())
            {
                foreach (var winLossByProductModel in reportData)
                {
                    foreach (var winLossByBetype in winLossByProductModel.WinLossByBetTypeData)
                    {
                        grandTotal.Add(winLossByBetype);
                    }
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