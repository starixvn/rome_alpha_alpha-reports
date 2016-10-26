namespace Alpha.Reports.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public class ProductServiceImpl : ProductService
    {
        private readonly IDynamicRepository repository;

        public ProductServiceImpl(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Product> GetVisibleProducts(GetVisibleProductsParam param)
        {
            var products = new List<Product>();
            foreach (var product in param.ProductSetting)
            {
                if (param.ProductPermissions[product.Permission](product.Id))
                {
                    products.Add(product);
                }
            }

            return products;
        }

        public List<Product> GetProductsByCustomer(GetProductByCustomerCriteria criteria)
        {
            var productList = CacheService
                .GetOrSet(
                $"Alpha.Reports.Services.Impl.ProdoctsOf{criteria.CustId}",
                () =>
                {
                    if (criteria == null || !criteria.IsNotValid())
                    {
                        return Enumerable.Empty<Product>().ToList();
                    }

                    return repository.Get<List<Product>>(criteria);
                },
                new CacheItemOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)));

            return productList;
        }
    }
}
