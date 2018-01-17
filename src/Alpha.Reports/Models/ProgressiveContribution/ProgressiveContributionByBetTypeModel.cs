namespace Alpha.Reports.Models.ProgressiveContribution
{
    public class ProgressiveContributionByBetTypeModel
    {
        public ProgressiveContributionByBetTypeModel()
        {
        }

        public ProgressiveContributionByBetTypeModel(
            int productId,
            int bettypeId,
            string bettypeName,
            int betCount,
            decimal turnover,
            decimal contributionAmount,
            decimal agentContribution,
            decimal masterContribution,
            decimal superContribution)
        {
            ProductId = productId;
            BetTypeId = bettypeId;
            BetTypeName = bettypeName;
            BetCount = betCount;
            Turnover = turnover;
            ContributionAmount = contributionAmount;
            AgentContribution = agentContribution;
            MasterContribution = masterContribution;
            SuperContribution = superContribution;
        }

        public int ProductId { get; }

        public int BetTypeId { get; }

        public string BetTypeName { get; }

        public int BetCount { get; private set; }

        public decimal Turnover { get; private set; }

        public decimal ContributionAmount { get; private set; }

        public decimal AgentContribution { get; private set; }

        public decimal MasterContribution { get; private set; }

        public decimal SuperContribution { get; private set; }

        public decimal CompanyContribution
        {
            get
            {
                return ContributionAmount + SuperContribution + MasterContribution + AgentContribution;
            }

            private set
            {
            }
        }

        public ProgressiveContributionByBetTypeModel Add(ProgressiveContributionByBetTypeModel contributionByBetType)
        {
            if (contributionByBetType == null)
            {
                return this;
            }

            BetCount += contributionByBetType.BetCount;
            Turnover += contributionByBetType.Turnover;
            ContributionAmount += contributionByBetType.ContributionAmount;
            AgentContribution += contributionByBetType.AgentContribution;
            MasterContribution += contributionByBetType.MasterContribution;
            SuperContribution += contributionByBetType.SuperContribution;
            CompanyContribution += contributionByBetType.CompanyContribution;

            return this;
        }
    }
}