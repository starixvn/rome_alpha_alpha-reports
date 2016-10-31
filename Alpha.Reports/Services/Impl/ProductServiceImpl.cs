namespace Alpha.Reports.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Caching;
    using Models;
    using RemotingRef;
    using Sunplus.Agent.Model.Entities.Permission;

    public class ProductServiceImpl : ProductService
    {
        private readonly SiteContext context;
        private readonly Func<int, bool> canViewProduct;
        private readonly Func<int, string> getProductName;

        public ProductServiceImpl() : this(null, null, null)
        {
        }

        internal ProductServiceImpl(
            SiteContext context,
            Func<int, bool> canViewProduct,
            Func<int, string> getProductName)
        {
            this.context = context;
            this.canViewProduct = canViewProduct ?? UserPermission.HasPermissionOnProduct;
            this.getProductName = getProductName ?? new CacheRefServiceImpl().GetProductName;
        }

        public IEnumerable<Product> GetProductsForCustomer(int custId)
        {
            var productList = CacheService
                .GetOrSet(
                $"Alpha.Reports.Services.Impl.ProductsForCustomer-{custId}",
                () => context
                        .Products
                        .Where(product => canViewProduct(product.Id) == true)
                        .Select(product => new Product(product.Id, product.Name, getProductName)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

            return productList;
        }
    }
}
