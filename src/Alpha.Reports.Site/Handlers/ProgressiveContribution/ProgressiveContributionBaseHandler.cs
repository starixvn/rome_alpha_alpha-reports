namespace Alpha.Reports.Site.Handlers.ProgressiveContribution
{
    using System.Collections.Generic;
    using System.Linq;
    using Params;
    using Reports.Models;
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
            var productIds = FilterProductsForJackpot(message.CustId, message.EnableJackpotProductIds).Select(p => p.Id);

            return new ProgressiveContributionParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                productIds,
                message.ViewerLevel);
        }

        protected IEnumerable<Product> FilterProductsForJackpot(int custId, IEnumerable<int> productIds)
            => productService.FilterProductsByCustomer(custId, productIds);

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