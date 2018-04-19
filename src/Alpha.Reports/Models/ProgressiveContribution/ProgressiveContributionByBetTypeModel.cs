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
            decimal turnover,
            decimal contributionAmount,
            decimal agentContribution,
            decimal masterContribution,
            decimal superContribution)
        {
            ProductId = productId;
            BetTypeId = bettypeId;
            BetTypeName = bettypeName;
            Turnover = turnover;
            ContributionAmount = contributionAmount;
            AgentContribution = agentContribution;
            MasterContribution = masterContribution;
            SuperContribution = superContribution;
        }

        public int ProductId { get; }

        public int BetTypeId { get; }

        public string BetTypeName { get; }

        public decimal Turnover { get; private set; }

        public decimal ContributionAmount { get; private set; }

        public decimal AgentContribution { get; private set; }

        public decimal MasterContribution { get; private set; }

        public decimal SuperContribution { get; private set; }

        public decimal CompanyContribution
        {
            get
            {
                return -(SuperContribution + MasterContribution + AgentContribution);
            }

            private set
            {
            }
        }

        public ProgressiveContributionByBetTypeModel Add(ProgressiveContributionByBetTypeModel contributionByBetType)
        {
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