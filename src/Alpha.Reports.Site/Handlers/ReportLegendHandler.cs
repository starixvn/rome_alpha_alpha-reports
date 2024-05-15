namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Configurations;
    using Alpha.Reports.Constants;
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
        private readonly ProductDetailService productDetailService;
        private readonly CacheRefService cacheRefService;

        public ReportLegendHandler(
            ProductService productServiceImpl,
            ProductDetailService productDetailService,
            CacheRefService cacheRefService,
            IUser viewer)
        {
            productService = productServiceImpl;
            this.cacheRefService = cacheRefService;
            this.productDetailService = productDetailService;
        }

        public IEnumerable<string> Handle(ReportLegendQuery message)
            => GetReportLegends(message.Site, message.CurrencyId, message.MinDate, message.IsSupportHistoricReport, message.IsHistoricReport);

        public ReportLegendViewModel Handle(ReportLegendGrossCommQuery message)
        {
            var legends = GetReportLegends(message.Site, message.CurrencyId, message.MinDate, message.IsSupportHistoricReport, message.IsHistoricReport).ToList();

            var productDetailBelongSabaOthers = productDetailService.GetProductDetailBelongSabaOtherSports(message.ViewerId, message.Site, message.CurrencyId);
            if (productDetailBelongSabaOthers.Any())
            {
                legends.Add(string.Format(Resources.SabaOtherSportsLegend,
                    string.Join(", ", productDetailBelongSabaOthers),
                    cacheRefService.GetProductDetailName(ProductDetaiIdConstants.SabaOtherSports)));
            }

            var legendsAfterGrossCommNotes = new List<string>
            {
                Resources.GrossCommRoundingNote
            };

            if (message.ShowDormantAccountsNote)
            {
                legendsAfterGrossCommNotes.Add(Resources.DormantAccountsNote);
            }

            return new ReportLegendViewModel(legends, GetGrossCommNotes(message.ViewerLevel), legendsAfterGrossCommNotes);
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