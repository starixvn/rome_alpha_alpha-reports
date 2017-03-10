namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.WinLossByBetTypeCriteria;
    using DataAccesses;
    using Models.WinLossByProduct;
    using Params;
    using RemotingRef;

    public interface WinLossByBetTypeService
    {
        IEnumerable<WinLossByProductModel> GetWinLossByBetType(WinLossByBetTypeParam param);
    }

    public class WinLossByBetTypeServiceImpl : WinLossByBetTypeService
    {
        private readonly ReportDateService reportDateService;
        private readonly WinLossByBetTypeDataAccess dataAccess;
        private readonly CacheRefService cacheRefService;

        public WinLossByBetTypeServiceImpl(
            ReportDateService reportDateService,
            WinLossByBetTypeDataAccess dataAccess,
             CacheRefService cacheRefService)
        {
            this.reportDateService = reportDateService;
            this.dataAccess = dataAccess;
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<WinLossByProductModel> GetWinLossByBetType(WinLossByBetTypeParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var criteria = new WinLossByBetTypeCriteria(
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
                    .Select(winLossByBetType => new WinLossByProductModel(
                    winLossByBetType.Key,
                    cacheRefService.GetProductName(winLossByBetType.Key),
                    false,
                    101,
                    56022,
                    566121,
                    winLossByBetType.FirstOrDefault().MemberNetWinLoss,
                    winLossByBetType.FirstOrDefault().AgentNetWinLoss,
                    winLossByBetType.FirstOrDefault().MasterNetWinLoss,
                    winLossByBetType.FirstOrDefault().SuperNetWinLoss,
                    winLossByBetType));
            }

            return Enumerable.Empty<WinLossByProductModel>();
        }
    }
}