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

            var legend = products.Any()
                ? string.Format(Resources.DelayIssueLegend, productService.GetProductNames(products).JoinWithComma())
                : Resources.NoInformationIsAvailable;

            return new List<string> { legend };
        }
    }
}