namespace Alpha.Reports.Site.Handlers.WinLossByProduct
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using App_Start;
    using Extensions;
    using Params;
    using Reports.Models;
    using Reports.Models.WinlossByBetType;
    using Reports.Models.WinLossByProduct;
    using Requests.WinLossByProduct;
    using Services;
    using Sunplus.Agent.Model.BL.Definitions;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class WinLossByProductBaseHandler : BaseHandler
    {
        protected readonly ProductService productService;
        protected readonly WinLossByProductService reportService;
        protected readonly CacheRefService cacheRefService;

        public WinLossByProductBaseHandler(WinLossByProductService reportService, ProductService productService, CacheRefService cacheRefService)
        {
            this.productService = productService;
            this.reportService = reportService;
            this.cacheRefService = cacheRefService;
        }

        protected WinLossByProductParam CreateWinLossByBettypeParamater(WinLossByProductQuery message)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(
                message.CustId,
                message.UserSelectedProductIds.ToListInt(),
                message.MinDate);

            return new WinLossByProductParam(
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
                foreach (var winLossByProduct in reportData)
                {
                    grandTotal.Add(winLossByProduct.Total);
                }
            }

            return grandTotal;
        }

        public string BuildDisplayedBetTypeName(int productId, BetType betTypeId, string betTypeName, bool liveIndicator)
        {
            if (BetTypeConfig.DisplayWinLossByBetTypeRules.ContainsKey(productId))
            {
                return BetTypeConfig.DisplayWinLossByBetTypeRules[productId](betTypeId, betTypeName, liveIndicator);
            }

            return betTypeName;
        }
    }
}