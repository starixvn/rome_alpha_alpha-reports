namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria;
    using Extensions;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;

    public interface ProductService
    {
        IEnumerable<Product> GetProgressiveJackpotProduct(int viewerId, string site, int currencyId, IEnumerable<int> progressiveJackpotproductIds);

        IEnumerable<Product> GetProductsForCustomer(int viewerId, string site, int currencyId, DateTime? minReportDate = null, bool isEnableSabaPromotion = false);

        IEnumerable<int> FilterProductIdsByCustomer(int viewerId, string site, int currencyId, IEnumerable<int> productIds, DateTime? minReportDate = null, bool isEnableSabaPromotion = false);

        IEnumerable<Product> FilterProductsByCustomer(int viewerId, string site, int currencyId, IEnumerable<int> productIds, DateTime? minReportDate = null, bool isEnableSabaPromotion = false);

        IEnumerable<string> GetProductNames(IEnumerable<int> productIds);

        string ConcatProductNames(string productIds);

        string ConcatProductNames(IEnumerable<int> productIds, IDictionary<int, string> replacedProductNames);

        IEnumerable<ProductsByRefTable> GetProductsNeedDelayTimeToSumarizeData();

        IEnumerable<byte> GetProductsNeedDelayTimeToSumarizeDataByCustomer(int viewerId, string site, int currencyId);

        bool CanViewBySite(int viewerId, string site, int currencyId, int productId);

        // Async methods
        Task<IEnumerable<Product>> GetProductsForCustomerAsync(
            int viewerId,
            string site,
            int currencyId,
            DateTime? minReportDate = null,
            bool isEnableSabaPromotion = false);
    }

    public class ProductServiceImpl : ProductService
    {
        private const int ProductsCacheInDay = 1;
        private const int CustProductsCacheTimeInMinutes = 5;
        private const string ProductsCacheKey = "Alpha.Reports.Services.Impl.ProductServiceImpl.Products";
        private readonly Func<int, bool> canViewProduct;
        private readonly Func<int, string> getProductName;
        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;
        private const int SabaPromotionProductId = 37;

        public ProductServiceImpl(
            IDynamicRepository dynamicRepository,
            ICacheService cacheService,
            Func<int, bool> canViewProduct,
            Func<int, string> getProductName)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
            this.canViewProduct = canViewProduct;
            this.getProductName = getProductName;
        }

        public IEnumerable<Product> GetProgressiveJackpotProduct(int viewerId, string site, int currencyId, IEnumerable<int> progressiveJackpotproductIds)
        {
            var offeredProducts = progressiveJackpotproductIds.Where(i => canViewProduct(i) || CanViewBySite(viewerId, site, currencyId, i));

            return offeredProducts.Select(i => new Product(i, getProductName(i), null, 0, i, getProductName));
        }

        public IEnumerable<int> FilterProductIdsByCustomer(int viewerId, string site, int currencyId, IEnumerable<int> productIds, DateTime? minReportDate = null, bool isEnableSabaPromotion = false)
        {
            var customerProductIds = GetProductsForCustomer(viewerId, site, currencyId, minReportDate, isEnableSabaPromotion).Select(product => product.Id);

            var productIdList = productIds == null || !productIds.Any()
                ? customerProductIds
                : customerProductIds.Where(product => productIds.Contains(product));

            return productIdList;
        }

        public IEnumerable<Product> FilterProductsByCustomer(int viewerId, string site, int currencyId, IEnumerable<int> productIds, DateTime? minReportDate = null, bool isEnableSabaPromotion = false)
            => GetProductsForCustomer(viewerId, site, currencyId, minReportDate, isEnableSabaPromotion).Where(product => productIds.Contains(product.Id));

        public IEnumerable<Product> GetProductsForCustomer(int viewerId, string site, int currencyId, DateTime? minReportDate = null, bool isEnableSabaPromotion = false)
        {
            if (viewerId <= 0)
            {
                return Enumerable.Empty<Product>();
            }

            var custProducts = cacheService
                .GetOrSet(
                $"Alpha.Reports.Services.Impl.ProductsForCustomer-{viewerId}",
                () => GetProductsForReport()
                    .Where(product => canViewProduct(product.Id) || CanViewBySite(viewerId, site, currencyId, product.Id))
                    .Select(product => new Product(product.Id, product.Name, product.EndOfSupportDate, product.Type, product.OrderNo, getProductName)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

            return GetProductsForCustomer(custProducts, minReportDate, isEnableSabaPromotion);
        }

        private IEnumerable<Product> GetProductsForCustomer(
            IEnumerable<Product> custProducts,
            DateTime? minReportDate,
            bool isEnableSabaPromotion,
            bool isCalculateSabaPromotion = true)
        {
            if (custProducts != null)
            {
                custProducts = custProducts.Where(product => IsRemainSupportProduct(product, minReportDate));

                if (isEnableSabaPromotion)
                {
                    custProducts = new[]
                    {
                        new Product(SabaPromotionProductId, null, isCalculateSabaPromotion ? custProducts.Max(product => product.EndOfSupportDate) : null, 0, 0, getProductName)
                    }.Concat(custProducts);
                }
            }

            return custProducts;
        }

        public IEnumerable<string> GetProductNames(IEnumerable<int> productIds)
        {
            var productNames = new List<string>();

            if (productIds == null)
            {
                return productNames;
            }

            foreach (var productId in productIds)
            {
                productNames.Add(getProductName(productId));
            }

            return productNames;
        }

        public string ConcatProductNames(string productIds)
            => string.Join(", ", GetProductNames(productIds.ToListInt()));

        public string ConcatProductNames(IEnumerable<int> productIds, IDictionary<int, string> replacedProductNames)
            => productIds == null
            ? string.Empty
            : string.Join(", ", productIds.Select(id => replacedProductNames.TryGetValue(id, out var name) ? name : getProductName(id)));

        public IEnumerable<ProductsByRefTable> GetProductsNeedDelayTimeToSumarizeData()
        {
            var data = cacheService
              .GetOrSet(
              "Alpha.Reports.Services.Impl.ProductsNeedDelayTimeToSumarizeData",
              () => dynamicRepository.Fetch<ProductsByRefTable>(new GetProductsByRefTableCriteria(null)),
              new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

            return data;
        }

        public IEnumerable<byte> GetProductsNeedDelayTimeToSumarizeDataByCustomer(int viewerId, string site, int currencyId)
        {
            var data = GetProductsNeedDelayTimeToSumarizeData();

            return data.Where(product => canViewProduct(product.ProductId) || CanViewBySite(viewerId, site, currencyId, product.ProductId)).Select(product => product.ProductId);
        }

        public bool CanViewBySite(int viewerId, string site, int currencyId, int productId)
        {
            var custProducts = cacheService
                .GetOrSet(
                $"Alpha.Reports.Services.Impl.ProductsBySite-{viewerId}",
                () => dynamicRepository.Fetch<ProductBySiteModel>(new GetOfferedProductsBySiteCriteria(viewerId, site, currencyId)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

            return custProducts.Any(i => i.ProductId == productId);
        }

        private static bool IsRemainSupportProduct(Product product, DateTime? minReportDate)
            => !minReportDate.HasValue
            || !product.EndOfSupportDate.HasValue
            || minReportDate.Value <= product.EndOfSupportDate.Value;

        private IEnumerable<Product> GetProductsForReport()
        {
            var products = cacheService
                .GetOrSet(
                ProductsCacheKey,
                () => dynamicRepository.Fetch<Product>(new GetProductsCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(ProductsCacheInDay)));

            return products;
        }

        // Async methods
        public async Task<IEnumerable<Product>> GetProductsForCustomerAsync(
            int viewerId,
            string site,
            int currencyId,
            DateTime? minReportDate = null,
            bool isEnableSabaPromotion = false)
        {
            if (viewerId <= 0)
            {
                return Enumerable.Empty<Product>();
            }

            var custProducts = await cacheService.GetOrSetAsync(
                $"Alpha.Reports.Services.Impl.ProductsForCustomer-{viewerId}",
                () => GetProductsForReport()
                    .Where(product => canViewProduct(product.Id) || CanViewBySite(viewerId, site, currencyId, product.Id))
                    .Select(product => new Product(product.Id, product.Name, product.EndOfSupportDate, product.Type, product.OrderNo, getProductName)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

            return GetProductsForCustomer(custProducts, minReportDate, isEnableSabaPromotion, false);
        }
    }
}