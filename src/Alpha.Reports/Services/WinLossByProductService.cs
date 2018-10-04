namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.WinLossByProductCriteria;
    using DataAccesses;
    using Models.WinlossByBetType;
    using Models.WinLossByProduct;
    using Params;

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
                param.UserSelectedProductIds.Concat(param.UserSelectedExcludeProductIds),
                param.CustLevel);

            var winLossByBetTypeData = dataAccess.GetWinLossByBetType(criteria);
            var result = new List<WinLossByProductModel>();

            if (winLossByBetTypeData.Any())
            {
                var winLossByProducts = winLossByBetTypeData.GroupBy(winLossByBetType => winLossByBetType.ProductId);

                foreach (var winLossByProduct in winLossByProducts)
                {
                    result.Add(new WinLossByProductModel(
                        winLossByProduct.Key,
                        cacheRefService.GetProductName(winLossByProduct.Key),
                        CreateWinLossBySportModel(winLossByProduct)));
                }
            }

            return result;
        }

        private static IEnumerable<WinLossBySportModel> CreateWinLossBySportModel(
            IGrouping<int, WinLossByBetTypeModel> winLossByProduct)
        {
            var winLossBySports = winLossByProduct.GroupBy(winLossByBetType => new
            {
                winLossByBetType.SportTypeId,
                winLossByBetType.SportTypeName
            });

            var result = new List<WinLossBySportModel>();

            foreach (var winLossBySport in winLossBySports)
            {
                result.Add(new WinLossBySportModel(
                    winLossBySport.Key.SportTypeId,
                    winLossBySport.Key.SportTypeName,
                    winLossBySport.ToList()));
            }

            return result;
        }
    }
}