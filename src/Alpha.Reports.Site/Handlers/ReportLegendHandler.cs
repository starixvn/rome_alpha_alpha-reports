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
            => GetReportLegends(message.ViewerId, message.Site, message.CurrencyId, message.MinDate, message.IsSupportHistoricReport, message.IsHistoricReport);

        public ReportLegendViewModel Handle(ReportLegendGrossCommQuery message)
        {
            var legends = GetReportLegends(message.ViewerId, message.Site, message.CurrencyId, message.MinDate, message.IsSupportHistoricReport, message.IsHistoricReport);

            return new ReportLegendViewModel(legends, GetGrossCommNotes(message.ViewerLevel));
        }

        private IEnumerable<string> GetReportLegends(int viewerId, string site, int currencyId, DateTime minDate, bool isSupportHistoricReport = false, bool isHistoricReport = false)
        {
            var result = new List<string>();
            var products = productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer(viewerId, site, currencyId).Select(product => (int)product);

            if (isSupportHistoricReport && !isHistoricReport)
            {
                result.Add(string.Format(
                    $"- {Resources.CanOnlyViewDataAfterTimeCurrentReport}", minDate.ToShortDateWithSlash()));
            }

            if (isSupportHistoricReport)
            {
                result.Add(string.Format(
                    $"- {string.Format(Resources.SabaSportsNote, SiteSettings.SeperateSabaSportsDate)}"));
            }

            if (products.Any())
            {
                result.Add(string.Format(Resources.DelayIssueLegend, string.Join(", ", productService.GetProductNames(products))));
            }

            return result;
        }
    }
}