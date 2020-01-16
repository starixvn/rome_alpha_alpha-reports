namespace Alpha.Reports.Site.ViewModels
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.App_GlobalResources;
    using System;
    using System.Collections.Generic;

    public class RiskAnalysisIPMatchingViewModel
    {
        public RiskAnalysisIPMatchingViewModel(
            DateTime winlostDate,
            IEnumerable<RiskAnalysisIPMatchingModel> riskAnalysisIPMatchingItems)
        {
            WinlostDate = winlostDate;
            RiskAnalysisIPMatchingItems = riskAnalysisIPMatchingItems;
        }

        public DateTime WinlostDate { get; }

        public IEnumerable<RiskAnalysisIPMatchingModel> RiskAnalysisIPMatchingItems { get; }

        public RiskAnalysisIPMatchingModel TotalRow {
            get {
                int memberCount = 0;
                int betCount = 0;
                decimal memberTurnover = 0;
                decimal memberWinLoss= 0;
                decimal superWinLoss = 0;
                decimal company = 0;

                foreach (var item in RiskAnalysisIPMatchingItems)
                {
                    memberCount += item.MemberCount;
                    betCount += item.BetCount;
                    memberTurnover += item.MemberTurnover;
                    memberWinLoss += item.MemberWinLoss;
                    superWinLoss += item.SuperWinLoss;
                    company += item.Company;
                }

                return new RiskAnalysisIPMatchingModel(
                    Resources.Total,
                    memberCount,
                    betCount,
                    memberTurnover,
                    memberWinLoss,
                    superWinLoss,
                    company);
            }
        }
    }
}