namespace Alpha.Reports.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;
    using Fanex.Caching;
    using Models;
    using RemotingRef;
    using Sunplus.Agent.Model.Entities.Permission;

    public class ProductServiceImpl : ProductService
    {
        private readonly ApplicationContext applicationContext;
        private readonly Func<int, bool> canViewProduct;
        private readonly Func<int, string> getProductName;

        public ProductServiceImpl(ApplicationContext applicationContext) : this(applicationContext, null, null)
        {
        }

        internal ProductServiceImpl(
            ApplicationContext applicationContext,
            Func<int, bool> canViewProduct,
            Func<int, string> getProductName)
        {
            this.applicationContext = applicationContext;
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

            var productList = CacheService
                .GetOrSet(
                $"Alpha.Reports.Services.Impl.ProductsForCustomer-{custId}",
                () => applicationContext
                        .Products
                        .Where(product => canViewProduct(product.Id) == true)
                        .Select(product => new Product(product.Id, product.Name, getProductName)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

            return productList;
        }
    }
}
