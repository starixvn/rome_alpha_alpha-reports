namespace Alpha.Reports.Site.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using Requests;
    using Services;

    public class ReportLegendHandler : IRequestHandler<ReportLegendQuery, IEnumerable<string>>
    {
        private readonly ProductService productService;

        public ReportLegendHandler(ProductService productServiceImpl)
        {
            productService = productServiceImpl;
        }

        public IEnumerable<string> Handle(ReportLegendQuery message)
        {
            var products = productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer().Select(product => (int)product);

            return products.Any()
                ? new List<string> { string.Format(Resources.DelayIssueLegend, string.Join(", ", productService.GetProductNames(products))) }
                : Enumerable.Empty<string>();
        }
    }
}