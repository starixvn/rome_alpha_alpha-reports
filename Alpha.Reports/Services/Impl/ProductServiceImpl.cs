namespace Alpha.Reports.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;
    using RemotingRef;
    using Sunplus.Agent.Model.Entities.Permission;

    public class ProductServiceImpl : ProductService
    {
        private const int CustProductsCacheTimeInMinutes = 5;
        private const int ProductsCacheTimeInMinutes = 60;
        private const string ProductsCacheKey = "Alpha.Reports.Services.Impl.ProductServiceImpl.Products";
        private readonly Func<int, bool> canViewProduct;
        private readonly Func<int, string> getProductName;
        private readonly IDynamicRepository dynamicRepository;

        public ProductServiceImpl(IDynamicRepository dynamicRepository) : this(dynamicRepository, null, null)
        {
        }

        internal ProductServiceImpl(
            IDynamicRepository dynamicRepository,
            Func<int, bool> canViewProduct,
            Func<int, string> getProductName)
        {
            this.dynamicRepository = dynamicRepository;
            this.canViewProduct = canViewProduct ?? UserPermission.HasPermissionOnProduct;
            this.getProductName = getProductName ?? new CacheRefServiceImpl().GetProductName;
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

            var products = CacheService
                .GetOrSet(
                ProductsCacheKey,
                () => dynamicRepository.Fetch<Product>(new GetProductsCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(ProductsCacheTimeInMinutes)));

            if (!products.Any())
            {
                return products;
            }

            var custProducts = CacheService
                .GetOrSet(
                $"Alpha.Reports.Services.Impl.ProductsForCustomer-{custId}",
                () => products
                        .Where(product => canViewProduct(product.Id) == true)
                        .Select(product => new Product(product.Id, product.Name, getProductName)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

            return custProducts;
        }
    }
}
