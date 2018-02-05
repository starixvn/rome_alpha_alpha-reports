namespace Alpha.Reports.Site.Handlers.ProgressiveContribution
{
    using System.Linq;
    using MediatR;
    using Requests.ProgressiveContribution;
    using Services;
    using ViewModels.ProgressiveContribution;

    public class ProgressiveContributionHandler :
        ProgressiveContributionBaseHandler,
        IRequestHandler<ProgressiveContributionQuery, ProgressiveContributionViewModel>
    {
        public ProgressiveContributionHandler(
            ProductService productService,
            ProgressiveContributionService progressiveContributionService)
            : base(productService, progressiveContributionService)
        {
        }

        public ProgressiveContributionViewModel Handle(ProgressiveContributionQuery message)
        {
            var products = productService.FilterProductsByCustomer(message.CustId, message.EnableJackpotProductIds);
            var param = CreateProgressiveContributionParam(message, products.Select(p => p.Id));

            var progressiveContributions = progressiveContributionService.GetProgressiveContributions(param);
            var grandTotal = AccumulateGrandTotal(progressiveContributions);

            return new ProgressiveContributionViewModel(
                message,
                param,
                BuildProductSelectListItems(param.UserSelectedProductIds, products),
                grandTotal,
                progressiveContributions);
        }
    }
}