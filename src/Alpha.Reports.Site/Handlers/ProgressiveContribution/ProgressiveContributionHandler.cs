namespace Alpha.Reports.Site.Handlers.ProgressiveContribution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels.Components;
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

            var products = productService.GetProgressiveJackpotProduct(message.ViewerId, message.Site, message.CurrencyId, message.EnableJackpotProductIds);
            var param = CreateProgressiveContributionParam(message);
            var progressiveContributions = progressiveContributionService.GetProgressiveContributions(param);
            var grandTotal = AccumulateGrandTotal(progressiveContributions);

            return new ProgressiveContributionViewModel(
                message,
                param,
                BuildGroupProductSelectListItems(param.UserSelectedProductIds, products),
                grandTotal,
                progressiveContributions);
        }

        public IEnumerable<string> Handle(ReportLegendQuery message)
        {
            var result = new List<string>();
            result.Add(Resources.ContributionAmountRoundNote);

            var productsNeedDelayTimeToSumarizeData = productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer(message.ViewerId, message.Site, message.CurrencyId)
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

        private static IEnumerable<GroupProductViewModel> BuildGroupProductSelectListItems(
            IEnumerable<int> userSelectedProductIds,
            IEnumerable<Product> products)
            => products?.Any() == true
            ? new List<GroupProductViewModel>
                {
                    new GroupProductViewModel(
                    new SelectListGroup(),
                    products.Select(product => new SelectListItem
                    {
                        Text = product.DisplayName,
                        Value = product.Id.ToString(),
                        Selected = userSelectedProductIds.Contains(product.Id)
                    }))
                }
            : Enumerable.Empty<GroupProductViewModel>();
    }
}
