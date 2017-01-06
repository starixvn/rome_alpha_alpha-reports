namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;

    public interface ProductService
    {
        IEnumerable<Product> GetProductsForCustomer(int custId);

        IEnumerable<int> FilterProductIdsByCustomer(int custId, IEnumerable<int> productIds);

        IEnumerable<string> GetProductNames(IEnumerable<int> productIds);
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

        public IEnumerable<int> FilterProductIdsByCustomer(int custId, IEnumerable<int> productIds)
        {
            var customerProductIds = GetProductsForCustomer(custId).Select(product => product.Id);

            var productIdList = productIds == null || !productIds.Any()
                ? customerProductIds
                : customerProductIds.Where(product => productIds.Contains(product));

            return productIdList;
        }

        public IEnumerable<Product> GetProductsForCustomer(int custId)
        {
            if (custId <= 0)
            {
                return Enumerable.Empty<Product>();
            }

            var custProducts = ConfiguredCacheService
                .GetOrSet(
                $"Alpha.Reports.Services.Impl.ProductsForCustomer-{custId}",
                () => GetProductsForReport()
                        .Where(product => canViewProduct(product.Id) == true)
                        .Select(product => new Product(product.Id, product.Name, getProductName)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

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