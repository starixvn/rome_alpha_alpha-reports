namespace Alpha.Reports.Site.Handlers.ProgressiveContribution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using App_GlobalResources;
    using MediatR;
    using Requests.ProgressiveContribution;
    using Services;
    using ViewModels.ProgressiveContribution;

    public class ProgressiveContributionHandler :
        ProgressiveContributionBaseHandler,
        IRequestHandler<ProgressiveContributionQuery, ProgressiveContributionViewModel>,
        IRequestHandler<ReportLegendQuery, IEnumerable<string>>
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

        public IEnumerable<string> Handle(ReportLegendQuery message)
        {
            var result = new List<string>();
            result.Add(Resources.ContributionAmountRoundNote);

            var productsNeedDelayTimeToSumarizeData = productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer()
                .Where(product => message.EnableJackpotProductIds.Contains(product))
                .Select(product => (int)product);

            if (productsNeedDelayTimeToSumarizeData.Any())
            {
                result.Add(string.Format(
                    Resources.DelayIssueLegend,
                    string.Join(", ", productService.GetProductNames(productsNeedDelayTimeToSumarizeData))));
            }

            return result;
        }
    }
}
