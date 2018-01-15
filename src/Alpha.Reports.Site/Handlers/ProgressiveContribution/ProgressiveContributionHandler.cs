namespace Alpha.Reports.Site.Handlers.ProgressiveContribution
{
    using System.Linq;
    using Configurations;
    using MediatR;
    using Requests.ProgressiveContribution;
    using Services;
    using ViewModels.ProgressiveContribution;

    public class ProgressiveContributionHandler :
        ProgressiveContributionBaseHandler,
        IRequestHandler<ProgressiveContributionQuery, ProgressiveContributionViewModel>
    {
        public ProgressiveContributionHandler(ProductService productService)
            : base(productService)
        {
        }

        public ProgressiveContributionViewModel Handle(ProgressiveContributionQuery message)
        {
            var products = productService.FilterProductsByCustomer(message.CustId, SiteSettings.SupportedJackPotProductIds);
            var param = CreateProgressiveContributionParam(message, products.Select(p => p.Id));

            return new ProgressiveContributionViewModel(
                message,
                param,
                BuildProductSelectListItems(param.UserSelectedProductIds, products));
        }
    }
}