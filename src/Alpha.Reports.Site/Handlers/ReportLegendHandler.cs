namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Configurations;
    using Alpha.Reports.Extensions;
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
            => GetReportLegends(message.Site, message.CurrencyId, message.MinDate, message.IsSupportHistoricReport, message.IsHistoricReport);

        public ReportLegendViewModel Handle(ReportLegendGrossCommQuery message)
        {
            var legends = GetReportLegends(message.Site, message.CurrencyId, message.MinDate, message.IsSupportHistoricReport, message.IsHistoricReport);

            return new ReportLegendViewModel(legends, GetGrossCommNotes(message.ViewerLevel));
        }

        private IEnumerable<string> GetReportLegends(string site, int currencyId, DateTime minDate, bool isSupportHistoricReport = false, bool isHistoricReport = false)
        {
            var result = new List<string>();
            var products = productService.GetOfferedProductsNeedDelayTimeToSumarizeData(site, currencyId, minDate).Select(product => product);

            if (isSupportHistoricReport && !isHistoricReport)
            {
                result.Add(string.Format(
                    $"- {Resources.CanOnlyViewDataAfterTimeCurrentReport}", minDate.ToShortDateWithSlash()));
            }

            if (products.Any())
            {
                result.Add(string.Format(Resources.DelayIssueLegend, string.Join(", ", productService.GetProductNames(products))));
            }

            return result;
        }
    }
}