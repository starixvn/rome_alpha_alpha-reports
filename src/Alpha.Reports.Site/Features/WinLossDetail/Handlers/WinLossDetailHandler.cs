namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Configurations;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Site.ViewModels.Components;
    using App_GlobalResources;
    using Fanex.Logging.Extensions.Web;
    using MediatR;
    using Reports.Constants;
    using Reports.Models;
    using Requests.WinLossDetail;
    using Services;
    using ViewModels.WinLossDetail;

    public class WinLossDetailHandler :
        WinLossDetailBaseHandler,
        IRequestHandler<WinLossDetailQuery, WinLossDetailViewModel>,
        IRequestHandler<ReportLegendQuery, ReportLegendViewModel>
    {
        private readonly IUser viewer;

        public WinLossDetailHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossDetailService reportService,
            WinLossService winLossService,
            IUser viewer) : base(productService, productDetailService, reportService, winLossService)
        {
            this.viewer = viewer;
        }

        public WinLossDetailViewModel Handle(WinLossDetailQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            CheckInvalidSessionCustIdsAndWriteLog(message);

            var products = productService.GetAllOfferedProducts(message.Site, message.CurrencyId, message.MinDate);
            var productDetails = productDetailService.GetProductDetails(message.ViewerId, message.Site, message.CurrencyId);

            var winLossDetailParam = CreateParameter(message, message.CustLevel);
            var reportData = winlossDetailService.GetWinLossDetail(winLossDetailParam);
            var reportEntered = reportService.GetEnteredSportResults();
            var viewModel = new WinLossDetailViewModel(
                message,
                winLossDetailParam,
                reportData,
                BuildBreadcrumbLevel(message),
                BuildProductSelectListItems(winLossDetailParam.UserSelectedProductIds, products),
                BuildProductDetailViewModels(winLossDetailParam.UserSelectedResourceIds, productDetails),
                totalEnteredSport: GetTotalEnteredSportYesterdayAndToday(reportEntered));

            return viewModel;
        }

        public ReportLegendViewModel Handle(ReportLegendQuery message)
        {
            message.CustId = viewer.Id;
            var result = new List<string>();

            if (message.IsNew)
            {
                result.Add(string.Format(
                    $"- {Resources.ReportDataCanViewNote}", message.MinDate.ToShortDateWithSlash()));
                result.Add(string.Format(
                    $"- {Resources.HistoryDataCanViewNote}", message.MinDate.ToShortDateWithSlash()));
            }
            else
            {
                if (message.IsSupportHistoricReport && !message.IsHistoricReport)
                {
                    result.Add(string.Format(
                        $"- {Resources.CanOnlyViewDataAfterTimeCurrentReport}", message.MinDate.ToShortDateWithSlash()));
                }
            }

            result.Add(Resources.Tooltip_NetTurnover);

            var liveCasinoTooltips = BuildTooltipsForLiveCasino(message.Site, message.CurrencyId, message.MinDate);
            if (!string.IsNullOrEmpty(liveCasinoTooltips))
            {
                result.Add(liveCasinoTooltips);
            }

            var rngCasinoTooltips = BuildTooltipsForRNGCasino(message.Site, message.CurrencyId, message.MinDate);
            if (!string.IsNullOrEmpty(rngCasinoTooltips))
            {
                result.Add(rngCasinoTooltips);
            }

            var productsNeedDelayTimeToSumarizeData = productService.GetOfferedProductsNeedDelayTimeToSumarizeData(message.Site, message.CurrencyId, message.MinDate).Select(product => (int)product);

            if (productsNeedDelayTimeToSumarizeData.Any())
            {
                result.Add(string.Format(
                    Resources.DelayIssueLegend,
                    string.Join(", ", productService.GetProductNames(productsNeedDelayTimeToSumarizeData))));
            }

            return new ReportLegendViewModel(result, GetGrossCommNotes(message.ViewerLevel), mobileFriendly: message.IsNew);
        }

        private string BuildTooltipsForLiveCasino(string site, int currencyId, DateTime? minDate)
        {
            var offeredLiveCasino = productService.FilterProducts(site, currencyId, ProductIdConstants.ProductListForTooltipsLiveCasino, minDate);
            var liveCasinoNames = string.Join(", ", offeredLiveCasino.Select(i => i.DisplayName));

            return offeredLiveCasino != null && offeredLiveCasino.Any() ?
                string.Format(Resources.Tooltip_NetTurnover_livecasino, liveCasinoNames) :
                string.Empty;
        }

        private string BuildTooltipsForRNGCasino(string site, int currencyId, DateTime? minDate)
        {
            var offeredRNGCasino = productService.GetAllOfferedProducts(site, currencyId, minDate)
                .Where(i => i.Type == ProductIdConstants.DummyRNGProductId || ProductIdConstants.ProductListForTooltipsRNGCasino.Contains(i.Id));
            var rngCasinoNames = string.Join(", ", offeredRNGCasino.Select(i => i.DisplayName));

            return offeredRNGCasino != null && offeredRNGCasino.Any() ?
                string.Format(Resources.Tooltip_NetTurnover_RNGCasino, rngCasinoNames) :
                string.Empty;
        }

        private static void CheckInvalidSessionCustIdsAndWriteLog(WinLossDetailQuery message)
        {
            if (SiteSettings.EnableLogDifferentSessionBetweenMainAndReports
                && !string.IsNullOrWhiteSpace(message.SSGUID))
            {
                var reportsSSGUID = $"{message.ViewerId}-{message.SubAccId}";

                if (message.SSGUID.IndexOf(reportsSSGUID, StringComparison.OrdinalIgnoreCase) == -1)
                {
                    ExceptionHandler.Handle(new Exception($"{message.SSGUID} # {reportsSSGUID}"));
                }
            }
        }
    }
}