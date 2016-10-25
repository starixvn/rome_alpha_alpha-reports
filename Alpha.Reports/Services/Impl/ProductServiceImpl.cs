namespace Alpha.Reports.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
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
            if (!criteria.IsValid())
            {
                return Enumerable.Empty<Product>().ToList();
            }

            var productList = repository.Get<List<Product>>(criteria);

            return productList;
        }
    }
}
