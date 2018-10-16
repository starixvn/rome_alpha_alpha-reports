namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Criteria.Consolidation;
    using Extensions;
    using Fanex.Data.Repository;
    using Models;
    using Models.Consolidation;
    using Params.Consolidation;

    public interface ConsolidationService
    {
        IEnumerable<CurrencyModel> GetCurrencies(GetCurrencyParam param);

        IEnumerable<Product> GetProducts(GetConslidationReportProductsParam param, IEnumerable<Product> excludeProducts);

        IEnumerable<int> FilterProductIdsByCustomer(int custId, IEnumerable<int> productIds, DateTime minReportDate, IEnumerable<Product> excludeProducts);

        IEnumerable<ConsolidationByUsername> GetConsolidationByUsername(GetConsolidationParam param);

        IEnumerable<ConsolidationByDate> GetConsolidationByDate(GetConsolidationParam param);

        IEnumerable<ConsolidationByProduct> GetConsolidationByProduct(GetConsolidationParam param);

        ConsolidationExcludeProduct GetConsolidationExcludeProduct(int custId, DateTime minReportDate);
    }

    public class ConsolidationServiceImpl : ConsolidationService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDateService reportDateService;
        private readonly CacheRefService cacheRefService;
        private readonly ProductService productService;

        public ConsolidationServiceImpl(
            IDynamicRepository dynamicRepository,
            ReportDateService reportDateService,
            CacheRefService cacheRefService,
            ProductService productService)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateService = reportDateService;
            this.cacheRefService = cacheRefService;
            this.productService = productService;
        }

        public IEnumerable<CurrencyModel> GetCurrencies(GetCurrencyParam param)
        {
            var data = dynamicRepository.Fetch<CurrencyModel>(new GetCurrencyCriteria(param.CustId));

            return data;
        }

        public IEnumerable<Product> GetProducts(GetConslidationReportProductsParam param, IEnumerable<Product> excludeProducts)
            => dynamicRepository.Fetch<ConsolidationProduct>(new GetConsolidationReportProductsCriteria(param.CustId))
                .Where(product => IsRemainSupportProduct(product, param.MinDate))
                .Select(product => new Product(product.Id, product.Name, product.EndOfSupportDate, cacheRefService.GetProductName))
                .Exclude(excludeProducts);

        public IEnumerable<ConsolidationByUsername> GetConsolidationByUsername(GetConsolidationParam param)
        {
            var criteria = new GetConsolidationCriteria(
                param,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                ConsolidationReportTypeConstants.Username);

            var data = dynamicRepository.Fetch<ConsolidationByUsername>(criteria);

            return data;
        }

        public IEnumerable<ConsolidationByDate> GetConsolidationByDate(GetConsolidationParam param)
        {
            var criteria = new GetConsolidationCriteria(
                param,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                ConsolidationReportTypeConstants.Daily);

            var data = dynamicRepository.Fetch<ConsolidationByDate>(criteria);

            return data;
        }

        public IEnumerable<ConsolidationByProduct> GetConsolidationByProduct(GetConsolidationParam param)
        {
            var criteria = new GetConsolidationCriteria(
                param,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                ConsolidationReportTypeConstants.Product);

            var data = dynamicRepository.Fetch<ConsolidationByProduct>(criteria);

            return data;
        }

        public IEnumerable<int> FilterProductIdsByCustomer(int custId, IEnumerable<int> productIds, DateTime minReportDate, IEnumerable<Product> excludeProducts)
            => GetProducts(new GetConslidationReportProductsParam(custId, minReportDate), excludeProducts)
                .Select(product => product.Id)
                .Where(id => productIds == null || !productIds.Any() || productIds.Contains(id));

        private static bool IsRemainSupportProduct(ConsolidationProduct product, DateTime minReportDate)
            => product.IsEnable
            && (!product.EndOfSupportDate.HasValue || minReportDate <= product.EndOfSupportDate.Value);

        public ConsolidationExcludeProduct GetConsolidationExcludeProduct(int custId, DateTime minReportDate)
        {
            var products = GetProducts(new GetConslidationReportProductsParam(custId, minReportDate), null).ToList();
            var excludeProducts = productService.GetExcludeProductsForCustomer(custId, minReportDate, true);

            if (excludeProducts.Any())
            {
                excludeProducts = products
                    .Where(product => excludeProducts.Any(excludeProduct => product.Id == excludeProduct.Id))
                    .ToList();
                products = products.Exclude(excludeProducts).ToList();
            }

            return new ConsolidationExcludeProduct(products, excludeProducts);
        }
    }
}
