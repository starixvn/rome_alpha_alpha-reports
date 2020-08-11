namespace Alpha.Reports.Site.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using App_GlobalResources;
    using MediatR;
    using Reports.Constants;
    using Reports.Models;
    using Requests;
    using Services;

    public class ReportLegendHandler : IRequestHandler<ReportLegendQuery, IEnumerable<string>>
    {
        private readonly ProductService productService;

        public ReportLegendHandler(ProductService productServiceImpl, IUser viewer)
        {
            productService = productServiceImpl;
        }

        public IEnumerable<string> Handle(ReportLegendQuery message)
        {
            var result = new List<string>();
            var products = productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer(message.Site, message.CurrencyId).Select(product => (int)product);

            if (products.Any())
            {
                result.Add(string.Format(Resources.DelayIssueLegend, string.Join(", ", productService.GetProductNames(products))));
            }

            return result;
        }
    }
}