namespace Alpha.Reports.Site.Handlers.Consolidation
{
    using System.Collections.Generic;
    using System.Linq;
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
        IRequestHandler<ConsolidationDetailQuery, ConsolidationDetailViewModel>
    {
        public ConsolidationHandler(
            ConsolidationService reportService,
            CacheRefService cacheRefService,
            WinLossDetailService winLossDetailService,
            IUser viewer) :
            base(reportService, cacheRefService, winLossDetailService, viewer)
        {
        }

        public ConsolidationReportByUsernameViewModel Handle(ConsolidationByUsernameQuery message)
        {
            var param = CreateParameter(message);
            var data = reportService.GetConsolidationByUsername(param);
            var products = reportService.GetProducts(new GetConslidationReportProductsParam(message.ViewerId, message.MinDate));
            var currencies = reportService.GetCurrencies(new GetCurrencyParam(message.ViewerId));

            var viewModel = new ConsolidationReportByUsernameViewModel(
                message,
                param,
                BuildProductSelectListItems(param.ProductIds, products),
                BuildCurrencySelectListItems(currencies, message.UserSelectedCurrencyId),
                BuildWinLossByUsernameViewModel(data, currencies.ToDictionary(c => c.Id, c => c.Name)));

            return viewModel;
        }

        public ConsolidationReportByDateViewModel Handle(ConsolidationByDateQuery message)
        {
            var param = CreateParameter(message);
            var data = reportService.GetConsolidationByDate(param);

            var products = reportService.GetProducts(new GetConslidationReportProductsParam(message.ViewerId, message.MinDate));
            var currencies = reportService.GetCurrencies(new GetCurrencyParam(message.ViewerId));

            var viewModel = new ConsolidationReportByDateViewModel(
                message,
                param,
                BuildProductSelectListItems(param.ProductIds, products),
                BuildCurrencySelectListItems(currencies, message.UserSelectedCurrencyId),
                BuildWinLossByDateViewModel(data, currencies.ToDictionary(c => c.Id, c => c.Name)));

            return viewModel;
        }

        public ConsolidationReportByProductViewModel Handle(ConsolidationByProductQuery message)
        {
            var param = CreateParameter(message);
            var data = reportService.GetConsolidationByProduct(param);

            var products = reportService.GetProducts(new GetConslidationReportProductsParam(message.ViewerId, message.MinDate));
            var currencies = reportService.GetCurrencies(new GetCurrencyParam(message.ViewerId));

            var viewModel = new ConsolidationReportByProductViewModel(
                message,
                param,
                BuildProductSelectListItems(param.ProductIds, products),
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
                IsStackMode = message.IsStackMode,
                IsFilterVisible = message.IsFilterVisible,
                UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,
                FromDate = message.FromDate,
                ToDate = message.ToDate,

                CustId = viewer.Id,
                CustName = viewer.Name,
                CustLevelId = viewer.LevelId
            };

            var winLossDetailQuery = CreateWinLossDetailQuery(message);

            return new ConsolidationDetailViewModel(
                message,
                GetConsolidationDetailBreadcrumbLevel(message),
                GetConsolidationFilterViewModel(consolidationQuery),
                GetWinLossDetailViewModel(winLossDetailQuery));
        }

        private ConsolidationFilterViewModel GetConsolidationFilterViewModel(ConsolidationByUsernameQuery query)
        {
            var param = CreateParameter(query);

            var products = reportService.GetProducts(new GetConslidationReportProductsParam(query.ViewerId, query.MinDate));
            var currencies = reportService.GetCurrencies(new GetCurrencyParam(query.ViewerId));

            var viewModel = new ConsolidationReportByUsernameViewModel(
                query,
                param,
                BuildProductSelectListItems(param.ProductIds, products),
                BuildCurrencySelectListItems(currencies, query.UserSelectedCurrencyId),
                null);

            return viewModel.FilterViewModel;
        }

        private WinLossDetailViewModel GetWinLossDetailViewModel(WinLossDetailQuery query)
        {
            var param = CreateWinLossDetailParam(query);

            var reportData = winLossDetailService.GetWinLossDetail(param);

            return new WinLossDetailViewModel(query, param, reportData, GetConsolidationDetailBreadcrumbLevel(query), null, isGroupHeadline: true);
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
    }
}