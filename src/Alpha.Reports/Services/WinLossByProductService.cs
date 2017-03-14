namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.WinLossByProductCriteria;
    using DataAccesses;
    using Models.WinLossByProduct;
    using Params;
    using RemotingRef;

    public interface WinLossByProductService
    {
        IEnumerable<WinLossByProductModel> GetWinLossByProduct(WinLossByProductParam param);
    }

    public class WinLossByProductServiceImpl : WinLossByProductService
    {
        private readonly ReportDateService reportDateService;
        private readonly WinLossByProductDataAccess dataAccess;
        private readonly CacheRefService cacheRefService;

        public WinLossByProductServiceImpl(
            ReportDateService reportDateService,
             WinLossByProductDataAccess dataAccess,
             CacheRefService cacheRefService)
        {
            this.reportDateService = reportDateService;
            this.dataAccess = dataAccess;
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<WinLossByProductModel> GetWinLossByProduct(WinLossByProductParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var criteria = new WinLossByProductCriteria(
             param.CustId,
             param.FromDate,
             param.ToDate,
             reportDateService.GetBalanceUpDay(),
             reportDateService.GetMoveBetsDay(),
             param.CustLevel);

            var winLossByBetTypeData = dataAccess.GetWinLossByBetType(criteria);

            if (winLossByBetTypeData.Any())
            {
                return winLossByBetTypeData.GroupBy(winLossByBetType => winLossByBetType.ProductId)
                    .Select(winLossByProduct => new WinLossByProductModel(
                    winLossByProduct.Key,
                    cacheRefService.GetProductName(winLossByProduct.Key),
                    winLossByProduct));
            }

            return Enumerable.Empty<WinLossByProductModel>();
        }
    }
}