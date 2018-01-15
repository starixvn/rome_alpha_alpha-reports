namespace Alpha.Reports.Site.Handlers.ProgressiveContribution
{
    using System.Collections.Generic;
    using Params;
    using Requests.ProgressiveContribution;
    using Services;

    public class ProgressiveContributionBaseHandler : BaseHandler
    {
        protected readonly ProductService productService;

        protected ProgressiveContributionBaseHandler(ProductService productService)
        {
            this.productService = productService;
        }

        protected static ProgressiveContributionParam CreateProgressiveContributionParam(ProgressiveContributionQuery message, IEnumerable<int> productIds)
            => new ProgressiveContributionParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                productIds,
                message.ViewerLevel);
    }
}