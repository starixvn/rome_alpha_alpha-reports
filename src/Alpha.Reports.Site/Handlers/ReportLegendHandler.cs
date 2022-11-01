namespace Alpha.Reports.Site.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Site.ViewModels.Components;
    using App_GlobalResources;
    using MediatR;
    using Reports.Models;
    using Requests;
    using Services;

    public class ReportLegendHandler :
        BaseHandler,
        IRequestHandler<ReportLegendQuery, IEnumerable<string>>,
        IRequestHandler<ReportLegendGrossCommQuery, ReportLegendViewModel>
    {
        private readonly ProductService productService;

        public ReportLegendHandler(ProductService productServiceImpl, IUser viewer)
        {
            productService = productServiceImpl;
        }

        public IEnumerable<string> Handle(ReportLegendQuery message)
            => GetReportLegends(message.ViewerId, message.Site, message.CurrencyId);

        public ReportLegendViewModel Handle(ReportLegendGrossCommQuery message)
        {
            var legends = GetReportLegends(message.ViewerId, message.Site, message.CurrencyId);

            return new ReportLegendViewModel(legends, GetGrossCommNotes(message.ViewerLevel));
        }

        private IEnumerable<string> GetReportLegends(int viewerId, string site, int currencyId)
        {
            var result = new List<string>();
            var products = productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer(viewerId, site, currencyId).Select(product => (int)product);

            if (products.Any())
            {
                result.Add(string.Format(Resources.DelayIssueLegend, string.Join(", ", productService.GetProductNames(products))));
            }

            return result;
        }
    }
}