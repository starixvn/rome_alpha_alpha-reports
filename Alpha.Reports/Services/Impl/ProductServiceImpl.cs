namespace Alpha.Reports.Services.Impl
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Fanex.Caching;
    using Models;
    using Sunplus.Agent.Model.Entities.Permission;
    using RemotingRef;

    public class ProductServiceImpl : ProductService
    {
        private readonly Func<int, bool> CanViewProduct;
        private readonly Func<int, string> GetProductName;

        public ProductServiceImpl(
            Func<int, bool> canViewProduct = null,
            Func<int, string> getProductName = null)
        {
            CanViewProduct = canViewProduct ?? UserPermission.HasPermissionOnProduct;
            GetProductName = getProductName ?? new CacheRefServiceImpl().GetProductName;
        }

        public IEnumerable<Product> GetProductsForCustomer(int custId, IEnumerable<Product> siteProducts)
        {
            var productList = CacheService
                .GetOrSet(
                $"Alpha.Reports.Services.Impl.ProductsForCustomer-{custId}", 
                () => siteProducts
                        .Where(product => CanViewProduct(product.Id) == true)
                        .Select(product => new Product(product.Id, GetProductName)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

            return productList;
        }
    }
}
