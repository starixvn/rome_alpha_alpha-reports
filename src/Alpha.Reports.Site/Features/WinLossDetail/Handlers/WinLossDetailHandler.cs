namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using Alpha.Reports.Configurations;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Site.ViewModels.Components;
    using Alpha.Reports.Utils;
    using App_GlobalResources;
    using Fanex.Logging.Extensions.Web;
    using MediatR;
    using Reports.Constants;
    using Reports.Models;
    using Requests.WinLossDetail;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels.WinLossDetail;

    public class WinLossDetailHandler :
        WinLossDetailBaseHandler,
        IRequestHandler<WinLossDetailQuery, WinLossDetailViewModel>,
        IRequestHandler<ReportLegendQuery, ReportLegendViewModel>
    {
        private readonly IUser viewer;

        private readonly CacheRefService cacheRefService;

        public WinLossDetailHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossDetailService reportService,
            WinLossService winLossService,
            CacheRefService cacheRefService,
            IUser viewer) : base(productService, productDetailService, reportService, winLossService)
        {
            this.viewer = viewer;
            this.cacheRefService = cacheRefService;
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
            var legends = new List<string>();

            if (message.IsNew)
            {
                var minDate = SystemTime.Today.AddYears(-3).FirstDayOfYear().ToShortDateWithSlash();
                legends.Add(string.Format(
                    $"- {Resources.ReportDataCanViewNote}", minDate));
            }
            else
            {
                if (message.IsSupportHistoricReport && !message.IsHistoricReport)
                {
                    legends.Add(string.Format(
                        $"- {Resources.CanOnlyViewDataAfterTimeCurrentReport}", message.MinDate.ToShortDateWithSlash()));
                }
            }

            legends.Add(Resources.Tooltip_NetTurnover);

            var liveCasinoTooltips = BuildTooltipsForLiveCasino(message.Site, message.CurrencyId, message.MinDate);
            if (!string.IsNullOrEmpty(liveCasinoTooltips))
            {
                legends.Add(liveCasinoTooltips);
            }

            var rngCasinoTooltips = BuildTooltipsForRNGCasino(message.Site, message.CurrencyId, message.MinDate);
            if (!string.IsNullOrEmpty(rngCasinoTooltips))
            {
                legends.Add(rngCasinoTooltips);
            }

            var productsNeedDelayTimeToSumarizeData = productService.GetOfferedProductsNeedDelayTimeToSumarizeData(message.Site, message.CurrencyId, message.MinDate).Select(product => (int)product);

            if (productsNeedDelayTimeToSumarizeData.Any())
            {
                legends.Add(string.Format(
                    Resources.DelayIssueLegend,
                    string.Join(", ", productService.GetProductNames(productsNeedDelayTimeToSumarizeData))));
            }

            var productDetailBelongSabaOthers = productDetailService.GetProductDetailBelongSabaOtherSports(message.ViewerId, message.Site, message.CurrencyId);
            if(productDetailBelongSabaOthers.Any())
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

            return new ReportLegendViewModel(
                legends,
                GetGrossCommNotes(message.ViewerLevel),
                legendsAfterGrossCommNotes,
                mobileFriendly: message.IsNew);
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