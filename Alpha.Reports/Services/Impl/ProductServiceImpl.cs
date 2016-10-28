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

        public List<Product> GetProductsByCustomer(ProductByCustomerParam param)
        {
            var criteria = new GetProductByCustomerCriteria(param.CustId);

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
