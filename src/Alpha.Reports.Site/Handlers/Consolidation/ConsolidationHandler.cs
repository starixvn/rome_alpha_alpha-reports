namespace Alpha.Reports.Site.Handlers.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Constants;
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
            ProductService productServiceImpl) :
            base(reportService, cacheRefService, viewer, productDetailService)
        {
            productService = productServiceImpl;
        }

        public ConsolidationReportByUsernameViewModel Handle(ConsolidationByUsernameQuery message)
        {
            var consolidationExcludeProduct = reportService.GetConsolidationExcludeProduct(message.CustId, message.MinDate, message.IsEnableSabaPromotionForHeadline);
            var currencies = reportService.GetCurrencies(new GetCurrencyParam(message.ViewerId));
            var productDetails = productDetailService.GetProductDetailsForConsolidationReport(message.CustId, consolidationExcludeProduct.Products);

            var param = CreateParameter(message, consolidationExcludeProduct.Products);
            var data = reportService.GetConsolidationByUsername(param);

            var viewModel = new ConsolidationReportByUsernameViewModel(
                message,
                param,
                BuildProductSelectListItems(param.ProductIds, consolidationExcludeProduct.Products),
                BuildProductDetailViewModels(param.ResourceIds, productDetails),
                BuildCurrencySelectListItems(currencies, message.UserSelectedCurrencyId),
                BuildWinLossByUsernameViewModel(data, currencies.ToDictionary(c => c.Id, c => c.Name)));

            return viewModel;
        }

        public ConsolidationReportByDateViewModel Handle(ConsolidationByDateQuery message)
        {
            var consolidationExcludeProduct = reportService.GetConsolidationExcludeProduct(message.CustId, message.MinDate, message.IsEnableSabaPromotionForHeadline);
            var currencies = reportService.GetCurrencies(new GetCurrencyParam(message.ViewerId));
            var productDetails = productDetailService.GetProductDetailsForConsolidationReport(message.CustId, consolidationExcludeProduct.Products);

            var param = CreateParameter(message, consolidationExcludeProduct.Products);
            var data = reportService.GetConsolidationByDate(param);

            var viewModel = new ConsolidationReportByDateViewModel(
                message,
                param,
                BuildProductSelectListItems(param.ProductIds, consolidationExcludeProduct.Products),
                BuildProductDetailViewModels(param.ResourceIds, productDetails),
                BuildCurrencySelectListItems(currencies, message.UserSelectedCurrencyId),
                BuildWinLossByDateViewModel(data, currencies.ToDictionary(c => c.Id, c => c.Name)));

            return viewModel;
        }

        public ConsolidationReportByProductViewModel Handle(ConsolidationByProductQuery message)
        {
            var consolidationExcludeProduct = reportService.GetConsolidationExcludeProduct(message.CustId, message.MinDate, message.IsEnableSabaPromotionForHeadline);
            var currencies = reportService.GetCurrencies(new GetCurrencyParam(message.ViewerId));
            var productDetails = productDetailService.GetProductDetailsForConsolidationReport(message.CustId, consolidationExcludeProduct.Products);

            var param = CreateParameter(message, consolidationExcludeProduct.Products);
            var data = reportService.GetConsolidationByProduct(param);

            var viewModel = new ConsolidationReportByProductViewModel(
                message,
                param,
                BuildProductSelectListItems(param.ProductIds, consolidationExcludeProduct.Products),
                BuildProductDetailViewModels(param.ResourceIds, productDetails),
                BuildCurrencySelectListItems(currencies, message.UserSelectedCurrencyId),
                BuildWinLossByProductViewModel(data, currencies.ToDictionary(c => c.Id, c => c.Name)));

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

            var products = reportService.GetProducts(
                new GetConslidationReportProductsParam(consolidationQuery.ViewerId, consolidationQuery.MinDate),
                message.IsEnableSabaPromotionForHeadline);
            var filterViewModel = GetConsolidationFilterViewModel(consolidationQuery, products);

            return new ConsolidationDetailViewModel(
                message,
                GetConsolidationDetailBreadcrumbLevel(message),
                filterViewModel,
                GetWinLossDetailViewModel(winLossDetailQuery, products));
        }

        public ReportLegendViewModel Handle(ConsolidationLegendQuery message)
        {
            var result = new List<string>();
            var products = reportService.GetProductsNeedDelayTimeToSumarizeDataByCustomer(message.CustId, message.MinDate, message.IsEnableSabaPromotionForHeadline);

            if (products.Any())
            {
                result.Add(string.Format(Resources.DelayIssueLegend, string.Join(", ", productService.GetProductNames(products))));
            }

            return new ReportLegendViewModel(result, GetConsolidationReportGrossCommNotes());
        }

        public ReportLegendViewModel Handle(ConsolidationDetailLegendQuery message)
        {
            var result = new List<string> { Resources.Tooltip_NetTurnover };

            var liveCasinoTooltips = BuildTooltipsForLiveCasino(message.CustId, message.MinDate, message.IsEnableSabaPromotionForHeadline);
            if (!string.IsNullOrEmpty(liveCasinoTooltips))
            {
                result.Add(liveCasinoTooltips);
            }

            var consolidationReportLegend = Handle((ConsolidationLegendQuery)message);
            result.AddRange(consolidationReportLegend.Legends);

            return new ReportLegendViewModel(result, GetGrossCommNotes(message.CustLevel));
        }

        private string BuildTooltipsForLiveCasino(int custId, DateTime minDate, bool isEnableSabaPromotion)
        {
            var offeredLiveCasino = reportService.FilterProductIdsByCustomer(custId, ProductIdConstants.ProductListForTooltipsLiveCasino, minDate, isEnableSabaPromotion);
            var liveCasinoNames = string.Join(", ", productService.GetProductNames(offeredLiveCasino));

            return offeredLiveCasino != null && offeredLiveCasino.Any() ?
                string.Format(Resources.Tooltip_NetTurnover_livecasino, liveCasinoNames) :
                string.Empty;
        }

        private ConsolidationFilterViewModel GetConsolidationFilterViewModel(
            ConsolidationByUsernameQuery query,
            IEnumerable<Product> products)
        {
            var currencies = reportService.GetCurrencies(new GetCurrencyParam(query.ViewerId));

            var productDetails = productDetailService.GetProductDetailsForConsolidationReport(query.ViewerId, products);

            var param = CreateParameter(query, products);

            var viewModel = new ConsolidationReportByUsernameViewModel(
                query,
                param,
                BuildProductSelectListItems(param.ProductIds, products),
                BuildProductDetailViewModels(param.ResourceIds, productDetails),
                BuildCurrencySelectListItems(currencies, query.UserSelectedCurrencyId),
                null);

            return viewModel.FilterViewModel;
        }

        private WinLossDetailViewModel GetWinLossDetailViewModel(WinLossDetailQuery query, IEnumerable<Product> products)
        {
            var param = CreateWinLossDetailParam(query, products);

            var reportData = reportService.GetWinLossDetail(param);

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

        private IEnumerable<string> GetConsolidationReportGrossCommNotes()
            => new List<string>
            {
                Resources.GrossCommNoteForConsolidationReportSuper,
                Resources.GrossCommNoteForConsolidationReportMaster,
                Resources.GrossCommNoteForConsolidationReportAgent,
                Resources.GrossCommNoteForCasino
            };
    }
}