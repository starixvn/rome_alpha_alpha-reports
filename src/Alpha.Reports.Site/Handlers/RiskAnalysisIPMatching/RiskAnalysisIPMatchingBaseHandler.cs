namespace Alpha.Reports.Site.Handlers
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Params;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Requests.RiskAnalysisIPMatching;
    using Alpha.Reports.Site.ViewModels;
    using AlphaPlus.Utilities.IP;
    using System.Collections.Generic;

    public class RiskAnalysisIPMatchingBaseHandler : BaseHandler
    {
        private readonly RiskAnalysisIPMatchingService riskAnalysisIPMatchingService;
        private readonly IPService ipService;

        public RiskAnalysisIPMatchingBaseHandler(
            RiskAnalysisIPMatchingService riskAnalysisIPMatchingService,
            IPService ipService)
        {
            this.riskAnalysisIPMatchingService = riskAnalysisIPMatchingService;
            this.ipService = ipService;
        }

        internal RiskAnalysisIPMatchingViewModel BuildRiskAnalysisIPMatchingViewModel(RiskAnalysisIPMatchingQuery message)
        {
            var riskAnalysisIPMatchingItems =
                riskAnalysisIPMatchingService.GetRiskAnalysisIPMatchingModel(
                   new RiskAnalysisIPMatchingParam(
                       message.CustId,
                       message.SelectedWinlostDate,
                       message.MoveBetsDate,
                       message.MemberCount));

            int memberCount = 0;
            int betCount = 0;
            decimal memberTurnover = 0;
            decimal memberWinLoss = 0;
            decimal superWinLoss = 0;
            decimal company = 0;

            var riskAnalysisIPMatchingItemsIPVN = new List<RiskAnalysisIPMatchingModel>();
            foreach (var item in riskAnalysisIPMatchingItems)
            {
                if (message.IsAdminCanViewVNIP || !ipService.IsVNIP(item.IP))
                {
                    memberCount += item.MemberCount;
                    betCount += item.BetCount;
                    memberTurnover += item.Stake;
                    memberWinLoss += item.MemberWinLoss;
                    superWinLoss += item.SuperWinLoss;
                    company += item.Company;

                    riskAnalysisIPMatchingItemsIPVN.Add(
                        new RiskAnalysisIPMatchingModel(
                           item.IP,
                           item.MemberCount,
                           item.BetCount,
                           item.Stake,
                           item.MemberWinLoss,
                           item.SuperWinLoss,
                           item.Company));
                }
            }

            return new RiskAnalysisIPMatchingViewModel(
                message.SelectedWinlostDate,
                message.MinDate,
                message.MemberCount,
                riskAnalysisIPMatchingItemsIPVN,
                new RiskAnalysisIPMatchingModel(
                    Resources.Total,
                    memberCount,
                    betCount,
                    memberTurnover,
                    memberWinLoss,
                    superWinLoss,
                    company));
        }
    }
}