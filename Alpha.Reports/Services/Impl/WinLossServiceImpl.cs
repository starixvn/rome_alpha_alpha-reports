namespace Alpha.Reports.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Alpha.RemotingRef;
    using AppResources;
    using Criteria.WinLoss;
    using DataAccesses;
    using Models;
    using Models.Winloss;
    using Models.WinlossDetails;
    using Params;

    public class WinLossServiceImpl : WinLossService
    {
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

        public ReportData<WinLoss> GetWinLoss(WinLossParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
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

            var winlossess = dataAccess.GetWinLosses(criteria);

            var totalWinLoss = BuildGrandTotal(winlossess, param.ReportLevel);

            return new ReportData<WinLoss>(winlossess, winlossess.Count, totalWinLoss); 
        }

        public ReportData<EnteredSportResult> GetEnteredSportResults()
        {
            var enteredResults = new List<EnteredSportResult>();
            List<EnteredSportDaily> enteredSportDailies = dataAccess.GetEnteredSportDailies();

            enteredResults = enteredSportDailies.GroupBy(x => x.SportTypeId)
                .Select(y => new EnteredSportResult(
                    y.Key,
                    CacheRef.GetSportName(y.Key),
                    y.ToList().Where(x => x.IsToday).FirstOrDefault(),
                    y.ToList().Where(x => !x.IsToday).FirstOrDefault())).ToList();
        
            return new ReportData<EnteredSportResult>(enteredResults);
        }

        private WinLoss BuildGrandTotal(List<WinLoss> winlossess, UserLevel reportLevel)
        {
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
