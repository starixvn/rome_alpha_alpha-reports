namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Criteria;
    using Alpha.Reports.Criteria.WinLossDetail;
    using Alpha.Reports.DataAccesses;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models.WinLossDetail;
    using Alpha.Reports.Params;
    using Constants;
    using Criteria.Consolidation;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;
    using Models.Consolidation;
    using Params.Consolidation;

    public interface ConsolidationService
    {
        IEnumerable<CurrencyModel> GetCurrencies(GetCurrencyParam param);

        IEnumerable<Product> GetProducts(GetConslidationReportProductsParam param);

        IEnumerable<int> FilterProductIdsByCustomer(int custId, IEnumerable<int> productIds, DateTime minReportDate);

        IEnumerable<ConsolidationByUsername> GetConsolidationByUsername(GetConsolidationParam param);

        IEnumerable<ConsolidationByDate> GetConsolidationByDate(GetConsolidationParam param);

        IEnumerable<ConsolidationByProduct> GetConsolidationByProduct(GetConsolidationParam param);

        ConsolidationExcludeProduct GetConsolidationExcludeProduct(int custId, DateTime minReportDate);

        PagedReportData<WinLossDetailModel> GetWinLossDetail(WinLossDetailParam param);

        IEnumerable<int> GetProductsNeedDelayTimeToSumarizeDataByCustomer(int custId, DateTime minReportDate);
    }

    public class ConsolidationServiceImpl : ConsolidationService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly CacheRefService cacheRefService;
        private readonly WinLossDetailDataAccess winLossDetailDataAccess;

        public ConsolidationServiceImpl(
            IDynamicRepository dynamicRepository,
            ReportDateService reportDateService,
            CacheRefService cacheRefService,
            WinLossDetailDataAccess winLossDetailDataAccess,
            ProductService productService)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateService = reportDateService;
            this.cacheRefService = cacheRefService;
            this.winLossDetailDataAccess = winLossDetailDataAccess;
            this.productService = productService;
        }

        public IEnumerable<CurrencyModel> GetCurrencies(GetCurrencyParam param)
        {
            var data = dynamicRepository.Fetch<CurrencyModel>(new GetCurrencyCriteria(param.CustId));

            return data;
        }

        public IEnumerable<Product> GetProducts(GetConslidationReportProductsParam param)
            => dynamicRepository.Fetch<ConsolidationProduct>(new GetConsolidationReportProductsCriteria(param.CustId))
                .Where(product => IsRemainSupportProduct(product, param.MinDate))
                .Select(product => new Product(product.Id, product.Name, product.EndOfSupportDate, product.Type, product.OrderNo, cacheRefService.GetProductName));

        public IEnumerable<ConsolidationByUsername> GetConsolidationByUsername(GetConsolidationParam param)
        {
            var criteria = new GetConsolidationCriteria(
                param,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                ConsolidationReportTypeConstants.Username,
                param.ProductDetailsXML);

            var data = dynamicRepository.Fetch<ConsolidationByUsername>(criteria);

            return data;
        }

        public IEnumerable<ConsolidationByDate> GetConsolidationByDate(GetConsolidationParam param)
        {
            var criteria = new GetConsolidationCriteria(
                param,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                ConsolidationReportTypeConstants.Daily,
                param.ProductDetailsXML);

            var data = dynamicRepository.Fetch<ConsolidationByDate>(criteria);

            return data;
        }

        public IEnumerable<ConsolidationByProduct> GetConsolidationByProduct(GetConsolidationParam param)
        {
            var criteria = new GetConsolidationCriteria(
                param,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                ConsolidationReportTypeConstants.Product,
                param.ProductDetailsXML);

            var data = dynamicRepository.Fetch<ConsolidationByProduct>(criteria);

            return data;
        }

        public IEnumerable<int> FilterProductIdsByCustomer(int custId, IEnumerable<int> productIds, DateTime minReportDate)
            => GetProducts(new GetConslidationReportProductsParam(custId, minReportDate))
                .Select(product => product.Id)
                .Where(id => productIds == null || !productIds.Any() || productIds.Contains(id));

        private static bool IsRemainSupportProduct(ConsolidationProduct product, DateTime minReportDate)
            => product.IsEnable
            && (!product.EndOfSupportDate.HasValue || minReportDate <= product.EndOfSupportDate.Value);

        public ConsolidationExcludeProduct GetConsolidationExcludeProduct(int custId, DateTime minReportDate)
        {
            var products = GetProducts(new GetConslidationReportProductsParam(custId, minReportDate)).ToList();

            return new ConsolidationExcludeProduct(products);
        }

        public PagedReportData<WinLossDetailModel> GetWinLossDetail(WinLossDetailParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var criteria = new WinLossDetailCriteria(
                param.CustId,
                param.PageIndex,
                param.PageSize,
                param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                param.UserSelectedProductIds,
                param.ReportLevel,
                param.ProductDetailsXML);

            var winLossDetailData = winLossDetailDataAccess.GetData(criteria);

            return winLossDetailData;
        }

        public IEnumerable<int> GetProductsNeedDelayTimeToSumarizeDataByCustomer(int custId, DateTime minReportDate)
        {
            var data = productService.GetProductsNeedDelayTimeToSumarizeData();

            return FilterProductIdsByCustomer(custId, data.Select(i => (int)i.ProductId), minReportDate);
        }
    }
}
