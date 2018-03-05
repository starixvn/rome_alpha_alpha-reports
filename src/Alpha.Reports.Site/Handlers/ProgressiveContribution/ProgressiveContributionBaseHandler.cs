namespace Alpha.Reports.Site.Handlers.ProgressiveContribution
{
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;
    using Params;
    using Reports.Models.ProgressiveContribution;
    using Requests.ProgressiveContribution;
    using Services;

    public class ProgressiveContributionBaseHandler : BaseHandler
    {
        protected readonly ProductService productService;
        protected readonly ProgressiveContributionService progressiveContributionService;

        protected ProgressiveContributionBaseHandler(
            ProductService productService,
            ProgressiveContributionService progressiveContributionService)
        {
            this.productService = productService;
            this.progressiveContributionService = progressiveContributionService;
        }

        protected ProgressiveContributionParam CreateProgressiveContributionParam(ProgressiveContributionQuery message)
        {
            var productIds = productService.FilterProductIdsByCustomer(
                message.CustId,
                message.UserSelectedProductIds.ToListInt(),
                message.MinDate);

            return new ProgressiveContributionParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                productIds,
                message.ViewerLevel);
        }

        internal static ProgressiveContributionByBetTypeModel AccumulateGrandTotal(IEnumerable<ProgressiveContributionByProductModel> reportData)
        {
            var grandTotal = new ProgressiveContributionByBetTypeModel();

            if (reportData != null && reportData.Any())
            {
                foreach (var progressiveContribution in reportData)
                {
                    grandTotal.Add(progressiveContribution.Total);
                }
            }

            return grandTotal;
        }
    }
}