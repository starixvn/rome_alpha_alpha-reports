namespace Alpha.Reports.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.RemotingRef;
    using AppResources;
    using Criteria.WinLoss;
    using DataAccesses;
    using Fanex.Caching;
    using Models;
    using Models.Winloss;
    using Models.WinlossDetails;
    using Params;

    public class WinLossServiceImpl : WinLossService
    {
        private const int EnteredResultsCacheTimeInMinutes = 5;
        private const string EnteredResultsCacheKey = "Alpha.Reports.Services.Impl.WinLossServiceImpl.EnteredResults";
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly WinLossDataAccess dataAccess;

        public WinLossServiceImpl(
            ReportDateService reportDateService,
            ProductService productService,
            WinLossDataAccess dataAccess)
        {
            this.reportDateService = reportDateService;
            this.productService = productService;
            this.dataAccess = dataAccess;
        }

        public ReportData<WinLoss> GetWinLosses(WinLossParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            if (!param.IsValid())
            {
                return new ReportData<WinLoss>(Enumerable.Empty<WinLoss>(), 0, new WinLoss());
            }

            var validUserSelectedProductIds = 
                productService.FilterProductIdsByCustomer(param.CustId, param.UserSelectedProductIds);

            var criteria = new WinLossCriteria(
                param.CustId,
                param.FromDate,
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                validUserSelectedProductIds,
                param.ReportLevel);

            var winlossess = dataAccess.GetWinLosses(criteria).OrderBy(x => x.CustName).ToList();

            var totalWinLoss = BuildGrandTotal(winlossess, param.ReportLevel);

            return new ReportData<WinLoss>(winlossess, winlossess.Count, totalWinLoss); 
        }

        public ReportData<EnteredSportResult> GetEnteredSportResults()
        {
            var reportData = CacheService
                 .GetOrSet(
                 EnteredResultsCacheKey,
                 () => GetDatabaseEnteredSportResults(),
                 new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

            return reportData;
        }

        private ReportData<EnteredSportResult> GetDatabaseEnteredSportResults()
        {
            var enteredResults = new List<EnteredSportResult>();
            List<EnteredSportDaily> enteredSportDailies = dataAccess.GetEnteredSportDailies();

            enteredResults = enteredSportDailies.GroupBy(x => x.SportTypeId)
                    .Select(y => new EnteredSportResult(
                    y.Key,
                    CacheRef.GetSportName(y.Key),
                    y.ToList().Where(x => x.IsToday).FirstOrDefault(),
                    y.ToList().Where(x => !x.IsToday).FirstOrDefault())).OrderBy(x => x.SportName).ToList();
        
            return new ReportData<EnteredSportResult>(enteredResults);
        }

        internal virtual WinLoss BuildGrandTotal(List<WinLoss> winlossess, UserLevel reportLevel)
        {
            if(winlossess == null || winlossess.Count().Equals(0) || reportLevel == null)
            {
                return new WinLoss();
            }

            var grantTotal = new WinLoss(
                 custId: 0,
                 custName: Resources.Total,
                 turnover: winlossess.Sum(x => x.Turnover),
                 grossComm: winlossess.Sum(x => x.GrossComm),
                 memberNetWinLoss: BuildGrandTotalMemberWinLoss(winlossess, reportLevel),
                 agentNetWinLoss: BuildGrandTotalAgentWinLoss(winlossess, reportLevel),
                 masterNetWinLoss: BuildGrandTotalMasterWinLoss(winlossess, reportLevel),
                 superNetWinLoss: BuildGrandTotalSuperWinLoss(winlossess, reportLevel));

            return grantTotal;
        }

        private NetWinLoss BuildGrandTotalMemberWinLoss(List<WinLoss> winlossess, UserLevel reportLevel)
        {
            var memberNetWinLoss = reportLevel.IsAgentLevel() ? new NetWinLoss(
                        winloss: winlossess.Sum(x => x.MemberNetWinLoss.WinLoss.Value),
                        comm: winlossess.Sum(x => x.MemberNetWinLoss.Comm.Value)) : null;

            return memberNetWinLoss;
        }

        private NetWinLoss BuildGrandTotalAgentWinLoss(List<WinLoss> winlossess, UserLevel reportLevel)
        {
            var agentNetWinLoss = reportLevel.IsNotSuperLevel() ?
                        (reportLevel.IsMasterLevel() ? new NetWinLoss(total: winlossess.Sum(x => x.AgentNetWinLoss.Total))
                        : new NetWinLoss(
                        winloss: winlossess.Sum(x => x.AgentNetWinLoss.WinLoss.Value),
                        comm: winlossess.Sum(x => x.AgentNetWinLoss.Comm.Value))) : null;

            return agentNetWinLoss;
        }

        private NetWinLoss BuildGrandTotalMasterWinLoss(List<WinLoss> winlossess, UserLevel reportLevel)
        {
            var masterNetWinLoss = reportLevel.IsMasterOrSuperLevel() ?
                        (reportLevel.IsMasterLevel() ?
                        new NetWinLoss(
                        winloss: winlossess.Sum(x => x.MasterNetWinLoss.WinLoss.Value),
                        comm: winlossess.Sum(x => x.MasterNetWinLoss.Comm.Value))
                        : new NetWinLoss(total: winlossess.Sum(x => x.MasterNetWinLoss.Total))) : null;

            return masterNetWinLoss;
        }

        private NetWinLoss BuildGrandTotalSuperWinLoss(List<WinLoss> winlossess, UserLevel reportLevel)
        {
            var superNetWinLoss = reportLevel.IsSuperLevel() ?
                        new NetWinLoss(
                        winloss: winlossess.Sum(x => x.SuperNetWinLoss.WinLoss.Value),
                        comm: winlossess.Sum(x => x.SuperNetWinLoss.Comm.Value)) : null;

            return superNetWinLoss;
        }
    }
}
