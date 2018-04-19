namespace Alpha.Reports.Models.ProgressiveContribution
{
    using System.Collections.Generic;

    public class ProgressiveContributionByProductModel
    {
        public ProgressiveContributionByProductModel(
            int productId,
            string productName,
            IEnumerable<ProgressiveContributionByBetTypeModel> progressiveContributions)
        {
            ProductId = productId;
            ProductName = productName;
            ProgressiveContributions = progressiveContributions;
            AccumulateTotal();
        }

        public int ProductId { get; }

        public string ProductName { get; }

        public IEnumerable<ProgressiveContributionByBetTypeModel> ProgressiveContributions { get; }

        public ProgressiveContributionByBetTypeModel Total { get; private set; }

        public ProgressiveContributionByProductModel AccumulateTotal()
        {
            Total = new ProgressiveContributionByBetTypeModel();

            foreach (var contribution in ProgressiveContributions)
            {
                Total.Add(contribution);
            }

            return this;
        }
    }
}