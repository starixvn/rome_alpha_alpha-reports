namespace Alpha.Reports.Site.Handlers.ProgressiveContribution
{
    using System;
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
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var products = productService.FilterProductsByCustomer(message.CustId, message.EnableJackpotProductIds, message.MinDate);

            var param = CreateProgressiveContributionParam(message);
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