namespace Alpha.Reports.Site.Handlers.Consolidation
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
    using Params.Consolidation;
    using Reports.Models;
    using Requests;
    using Requests.Consolidation;
    using Requests.WinLossDetail;
    using Services;
    using ViewModels.Consolidation;
    using ViewModels.WinLossDetail;

    public class ConsolidationHandler :
        ConsolidationBaseHandler,
        IRequestHandler<ConsolidationByUsernameQuery, ConsolidationReportByUsernameViewModel>,
        IRequestHandler<ConsolidationByDateQuery, ConsolidationReportByDateViewModel>,
        IRequestHandler<ConsolidationByProductQuery, ConsolidationReportByProductViewModel>,
        IRequestHandler<ConsolidationDetailQuery, ConsolidationDetailViewModel>,
        IRequestHandler<ConsolidationLegendQuery, ReportLegendViewModel>,
        IRequestHandler<ConsolidationDetailLegendQuery, ReportLegendViewModel>
    {
        private readonly ProductService productService;

        public ConsolidationHandler(
            ConsolidationService reportService,
            CacheRefService cacheRefService,
            IUser viewer,
            ProductDetailService productDetailService,
            ProductService productServiceImpl,
            WinLossService winLossService) :
            base(reportService, cacheRefService, viewer, productDetailService, winLossService)
        {
            productService = productServiceImpl;
        }

        public ConsolidationReportByUsernameViewModel Handle(ConsolidationByUsernameQuery message)
        {
            var consolidationExcludeProduct = consolidationService.GetConsolidationExcludeProduct(message.CustId, message.MinDate);
            var currencies = consolidationService.GetCurrencies(new GetCurrencyParam(message.ViewerId));
            var productDetails = productDetailService.GetProductDetailsForConsolidationReport(message.CustId, consolidationExcludeProduct.Products);

            var param = CreateParameter(message, consolidationExcludeProduct.Products);
            var data = consolidationService.GetConsolidationByUsername(param).ToList();
            var reportEntered = reportService.GetEnteredSportResults();

            return new ConsolidationReportByUsernameViewModel(
                message,
                param,
                BuildProductSelectListItems(param.ProductIds, consolidationExcludeProduct.Products),
                BuildProductDetailViewModels(param.ResourceIds, productDetails),
                BuildCurrencySelectListItems(currencies, message.UserSelectedCurrencyId),
                data.Any()
                    ? BuildConsolidationByUsernameViewModel(data, currencies.ToDictionary(c => c.Id, c => c.Name)).ToList()
                    : Enumerable.Empty<ConsolidationByUsernameViewModel>(),
                GetTotalEnteredSportYesterdayAndToday(reportEntered)
            );
        }

        public ConsolidationReportByDateViewModel Handle(ConsolidationByDateQuery message)
        {
            var consolidationExcludeProduct = consolidationService.GetConsolidationExcludeProduct(message.CustId, message.MinDate);
            var currencies = consolidationService.GetCurrencies(new GetCurrencyParam(message.ViewerId));
            var productDetails = productDetailService.GetProductDetailsForConsolidationReport(message.CustId, consolidationExcludeProduct.Products);

            var param = CreateParameter(message, consolidationExcludeProduct.Products);
            var data = consolidationService.GetConsolidationByDate(param);
            var reportEntered = reportService.GetEnteredSportResults();
            var viewModel = new ConsolidationReportByDateViewModel(
                message,
                param,
                BuildProductSelectListItems(param.ProductIds, consolidationExcludeProduct.Products),
                BuildProductDetailViewModels(param.ResourceIds, productDetails),
                BuildCurrencySelectListItems(currencies, message.UserSelectedCurrencyId),
                BuildWinLossByDateViewModel(data, currencies.ToDictionary(c => c.Id, c => c.Name)),
                GetTotalEnteredSportYesterdayAndToday(reportEntered));

            return viewModel;
        }

        public ConsolidationReportByProductViewModel Handle(ConsolidationByProductQuery message)
        {
            var consolidationExcludeProduct = consolidationService.GetConsolidationExcludeProduct(message.CustId, message.MinDate);
            var currencies = consolidationService.GetCurrencies(new GetCurrencyParam(message.ViewerId));
            var productDetails = productDetailService.GetProductDetailsForConsolidationReport(message.CustId, consolidationExcludeProduct.Products);

            var param = CreateParameter(message, consolidationExcludeProduct.Products);
            var data = consolidationService.GetConsolidationByProduct(param);
            var reportEntered = reportService.GetEnteredSportResults();

            var viewModel = new ConsolidationReportByProductViewModel(
                message,
                param,
                BuildProductSelectListItems(param.ProductIds, consolidationExcludeProduct.Products),
                BuildProductDetailViewModels(param.ResourceIds, productDetails),
                BuildCurrencySelectListItems(currencies, message.UserSelectedCurrencyId),
                BuildWinLossByProductViewModel(data, currencies.ToDictionary(c => c.Id, c => c.Name)),
                GetTotalEnteredSportYesterdayAndToday(reportEntered));

            return viewModel;
        }

        public ConsolidationDetailViewModel Handle(ConsolidationDetailQuery message)
        {
            var consolidationQuery = new ConsolidationByUsernameQuery(viewer)
            {
                UserSelectedCurrencyId = message.UserSelectedCurrencyId,
                UserSelectedProductIds = message.UserSelectedProductIds,
                UserSelectedResourceIds = message.UserSelectedResourceIds,
                IsStackMode = message.IsStackMode,
                IsFilterVisible = message.IsFilterVisible,
                UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,
                FromDate = message.FromDate,
                ToDate = message.ToDate,

                CustId = viewer.Id,
                CustName = viewer.Name,
                CustLevelId = viewer.LevelId,

                IsPostBack = message.IsPostBack
            };

            var winLossDetailQuery = CreateWinLossDetailQuery(message);

            var products = consolidationService.GetProducts(
                new GetConslidationReportProductsParam(consolidationQuery.ViewerId, consolidationQuery.MinDate));
            var filterViewModel = GetConsolidationFilterViewModel(consolidationQuery, products);

            var reportEntered = reportService.GetEnteredSportResults();
            
            return new ConsolidationDetailViewModel(
                message,
                GetConsolidationDetailBreadcrumbLevel(message),
                filterViewModel,
                GetWinLossDetailViewModel(winLossDetailQuery, products),
                GetTotalEnteredSportYesterdayAndToday(reportEntered));
        }

        public ReportLegendViewModel Handle(ConsolidationLegendQuery message)
        {
            var result = new List<string>();

            if (message.IsSupportHistoricReport && !message.IsHistoricReport)
            {
                result.Add(string.Format(
                    $"- {Resources.CanOnlyViewDataAfterTimeCurrentReport}", message.MinDate.ToShortDateWithSlash()));
            }

            if (message.IsSupportHistoricReport)
            {
                result.Add(string.Format(
                    $"- {string.Format(Resources.SabaSportsNote, SiteSettings.SeperateSabaSportsDate)}"));
            }

            var products = consolidationService.GetProductsNeedDelayTimeToSumarizeDataByCustomer(message.CustId, message.MinDate);

            if (products.Any())
            {
                result.Add(string.Format(Resources.DelayIssueLegend, string.Join(", ", productService.GetProductNames(products))));
            }

            return new ReportLegendViewModel(result, GetConsolidationReportGrossCommNotes(message.ViewerLevel));
        }

        public ReportLegendViewModel Handle(ConsolidationDetailLegendQuery message)
        {
            var result = new List<string>();

            if (message.IsSupportHistoricReport && !message.IsHistoricReport)
            {
                result.Add(string.Format(
                    $"- {Resources.CanOnlyViewDataAfterTimeCurrentReport}", message.MinDate.ToShortDateWithSlash()));
            }

            if (message.IsSupportHistoricReport)
            {
                result.Add(string.Format(
                    $"- {string.Format(Resources.SabaSportsNote, SiteSettings.SeperateSabaSportsDate)}"));
            }

            result.Add(Resources.Tooltip_NetTurnover);

            var liveCasinoTooltips = BuildTooltipsForLiveCasino(message.CustId, message.MinDate);
            if (!string.IsNullOrEmpty(liveCasinoTooltips))
            {
                result.Add(liveCasinoTooltips);
            }

            message.IsSupportHistoricReport = false;
            message.IsHistoricReport = false;

            var consolidationReportLegend = Handle((ConsolidationLegendQuery)message);
            result.AddRange(consolidationReportLegend.Legends);

            return new ReportLegendViewModel(result, GetGrossCommNotes(message.CustLevel));
        }

        private string BuildTooltipsForLiveCasino(int custId, DateTime minDate)
        {
            var offeredLiveCasino = consolidationService.FilterProductIdsByCustomer(custId, ProductIdConstants.ProductListForTooltipsLiveCasino, minDate);
            var liveCasinoNames = string.Join(", ", productService.GetProductNames(offeredLiveCasino));

            return offeredLiveCasino != null && offeredLiveCasino.Any() ?
                string.Format(Resources.Tooltip_NetTurnover_livecasino, liveCasinoNames) :
                string.Empty;
        }

        private ConsolidationFilterViewModel GetConsolidationFilterViewModel(
            ConsolidationByUsernameQuery query,
            IEnumerable<Product> products)
        {
            var currencies = consolidationService.GetCurrencies(new GetCurrencyParam(query.ViewerId));

            var productDetails = productDetailService.GetProductDetailsForConsolidationReport(query.ViewerId, products);

            var param = CreateParameter(query, products);
            var reportEntered = reportService.GetEnteredSportResults();

            var viewModel = new ConsolidationReportByUsernameViewModel(
                query,
                param,
                BuildProductSelectListItems(param.ProductIds, products),
                BuildProductDetailViewModels(param.ResourceIds, productDetails),
                BuildCurrencySelectListItems(currencies, query.UserSelectedCurrencyId),
                null,
                GetTotalEnteredSportYesterdayAndToday(reportEntered));

            return viewModel.FilterViewModel;
        }

        private WinLossDetailViewModel GetWinLossDetailViewModel(WinLossDetailQuery query, IEnumerable<Product> products)
        {
            var param = CreateWinLossDetailParam(query, products);

            var reportData = consolidationService.GetWinLossDetail(param);

            return new WinLossDetailViewModel(
                query,
                param,
                reportData,
                GetConsolidationDetailBreadcrumbLevel(query),
                null,
                null,
                isGroupHeadline: true);
        }

        private IEnumerable<CustInfo> GetConsolidationDetailBreadcrumbLevel(ReportBaseQuery query)
        {
            var breadcrumbLevel = new List<CustInfo>();
            breadcrumbLevel.Add(new CustInfo(query.ViewerId, Resources.Consolidation, query.ViewerLevelId));

            if (query.BreadcrumbLevel != null
                && query.BreadcrumbLevel.Any())
            {
                var requestBreadcrumb = query.BreadcrumbLevel.Skip(1);

                foreach (var custInfo in requestBreadcrumb)
                {
                    if (custInfo.CustLevelId == query.CustLevelId)
                    {
                        break;
                    }

                    breadcrumbLevel.Add(custInfo);
                }
            }

            breadcrumbLevel.Add(new CustInfo(query.CustId, query.CustName, query.CustLevelId));

            return breadcrumbLevel;
        }

        private IEnumerable<string> GetConsolidationReportGrossCommNotes(UserLevel viewerLevel)
        {
            yield return Resources.GrossCommNoteForConsolidationReportSuper;
            yield return Resources.GrossCommNoteForConsolidationReportMaster;
            yield return Resources.GrossCommNoteForConsolidationReportAgent;
            yield return Resources.GrossCommNoteForCasino;

            if (viewerLevel.IsNotAgentLevel())
            {
                yield return Resources.GrossCommNoteForRoundingProblem;
            }
        }
    }
}