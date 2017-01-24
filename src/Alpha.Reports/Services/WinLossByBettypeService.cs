namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.WinLoss;
    using DataAccesses;
    using Models;
    using Models.WinlossByBettype;
    using Params;
    using RemotingRef;

    public interface WinLossByBettypeService
    {
        IEnumerable<WinlossByBettypeModel> GetWinLossByBettype(WinLossByBettypeParam param);

        DateTime GetMinDate();
    }

    public class WinLossByBettypeServiceImpl : WinLossByBettypeService
    {
        private readonly ReportDateService reportDateService;
        private readonly WinLossByBettypeDataAccess dataAccess;
        private readonly CacheRefService cacheRefService;

        public WinLossByBettypeServiceImpl(
            ReportDateService reportDateService,
            WinLossByBettypeDataAccess dataAccess,
            CacheRefService cacheRefService)
        {
            this.reportDateService = reportDateService;
            this.dataAccess = dataAccess;
            this.cacheRefService = cacheRefService;
        }

        internal static DateRangeType DateRangeType { get; set; }

        public IEnumerable<WinlossByBettypeModel> GetWinLossByBettype(WinLossByBettypeParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            if (!reportDateService.ValidateDateRange(
                new DateRange(DateRangeType, param.FromDate, param.ToDate, false)))
            {
                return Enumerable.Empty<WinlossByBettypeModel>();
            }

            var criteria = new WinLossByBettypeCriteria(
                param.CustId,
                param.FromDate,
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                param.CustLevel);

            var winlossData = dataAccess.GetWinLossByBettype(criteria);

            return winlossData;
        }

        public DateTime GetMinDate() => reportDateService.GetBalanceUpDay();
    }
}