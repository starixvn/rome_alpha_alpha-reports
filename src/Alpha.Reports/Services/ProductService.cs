namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using Extensions;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;

    public interface ProductService
    {
        IEnumerable<Product> GetProductsForCustomer(int custId, DateTime? minReportDate = null);

        IEnumerable<int> FilterProductIdsByCustomer(int custId, IEnumerable<int> productIds, DateTime? minReportDate = null);

        IEnumerable<Product> FilterProductsByCustomer(int custId, IEnumerable<int> productIds, DateTime? minReportDate = null);

        IEnumerable<string> GetProductNames(IEnumerable<int> productIds);

        string ConcatProductNames(string productIds);

        IEnumerable<ProductsByRefTable> GetProductsNeedDelayTimeToSumarizeData();

        IEnumerable<byte> GetProductsNeedDelayTimeToSumarizeDataByCustomer();

        bool IsShowProduct(int custId, int productId);
    }

    public class ProductServiceImpl : ProductService
    {
        private const int CustProductsCacheTimeInMinutes = 5;
        private const string ProductsCacheKey = "Alpha.Reports.Services.Impl.ProductServiceImpl.Products";
        private readonly Func<int, bool> canViewProduct;
        private readonly Func<int, string> getProductName;
        private readonly IDynamicRepository dynamicRepository;

        public ProductServiceImpl(
            IDynamicRepository dynamicRepository,
            Func<int, bool> canViewProduct,
            Func<int, string> getProductName)
        {
            this.dynamicRepository = dynamicRepository;
            this.canViewProduct = canViewProduct;
            this.getProductName = getProductName;
        }

        public IEnumerable<int> FilterProductIdsByCustomer(int custId, IEnumerable<int> productIds, DateTime? minReportDate = null)
        {
            var customerProductIds = GetProductsForCustomer(custId, minReportDate).Select(product => product.Id);

            var productIdList = productIds == null || !productIds.Any()
                ? customerProductIds
                : customerProductIds.Where(product => productIds.Contains(product));

            return productIdList;
        }

        public IEnumerable<Product> FilterProductsByCustomer(int custId, IEnumerable<int> productIds, DateTime? minReportDate = null)
            => GetProductsForCustomer(custId, minReportDate).Where(product => productIds.Contains(product.Id));

        public IEnumerable<Product> GetProductsForCustomer(int custId, DateTime? minReportDate = null)
        {
            if (custId <= 0)
            {
                return Enumerable.Empty<Product>();
            }

            var custProducts = ConfiguredCacheService
                .GetOrSet(
                $"Alpha.Reports.Services.Impl.ProductsForCustomer-{custId}",
                () => GetProductsForReport()
                    .Where(product => canViewProduct(product.Id))
                    .Select(product => new Product(product.Id, product.Name, product.EndOfSupportDate, getProductName)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));
            if (custProducts != null)
            {
                custProducts = custProducts
                    .Where(product => IsRemainSupportProduct(product, minReportDate));
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

        public IEnumerable<ProductsByRefTable> GetProductsNeedDelayTimeToSumarizeData()
        {
            var data = ConfiguredCacheService
              .GetOrSet(
              "Alpha.Reports.Services.Impl.ProductsNeedDelayTimeToSumarizeData",
              () => dynamicRepository.Fetch<ProductsByRefTable>(new GetProductsByRefTableCriteria(null)),
              new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

            return data;
        }

        public IEnumerable<byte> GetProductsNeedDelayTimeToSumarizeDataByCustomer()
        {
            var data = GetProductsNeedDelayTimeToSumarizeData();

            return data.Where(product => canViewProduct(product.ProductId)).Select(product => product.ProductId);
        }

        public bool IsShowProduct(int custId, int productId)
            => GetProductsForCustomer(custId).Any(p => p.Id == productId);

        private static bool IsRemainSupportProduct(Product product, DateTime? minReportDate)
            => !minReportDate.HasValue
            || !product.EndOfSupportDate.HasValue
            || minReportDate.Value <= product.EndOfSupportDate.Value;

        private IEnumerable<Product> GetProductsForReport()
        {
            var products = ConfiguredCacheService
                .GetOrSet(
                ProductsCacheKey,
                () => dynamicRepository.Fetch<Product>(new GetProductsCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

            return products;
        }
    }
}