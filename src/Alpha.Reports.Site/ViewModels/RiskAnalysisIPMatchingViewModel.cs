namespace Alpha.Reports.Site.ViewModels
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.App_GlobalResources;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class RiskAnalysisIPMatchingViewModel
    {
        private IEnumerable<int> MemberCountItems
               => new List<int> { 2, 3, 4, 8, 10, 15, 20, 30, 50, 100 };

        public RiskAnalysisIPMatchingViewModel(
            DateTime winlostDate,
            int userSelectedStatusId,
            IEnumerable<RiskAnalysisIPMatchingModel> riskAnalysisIPMatchingItems)
        {
            WinlostDate = winlostDate;
            RiskAnalysisIPMatchingItems = riskAnalysisIPMatchingItems;
            StatusSelection = MemberCountItems.Select(item => new SelectListItem
            {
                Text = item.ToString(),
                Value = item.ToString(),
                Selected = item.Equals(userSelectedStatusId)
            });
        }

        public DateTime WinlostDate { get; }

        public IEnumerable<RiskAnalysisIPMatchingModel> RiskAnalysisIPMatchingItems { get; }

        public IEnumerable<SelectListItem> StatusSelection { get; }

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