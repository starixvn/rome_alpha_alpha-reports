namespace Alpha.Reports.Services.Impl
{
    using System.Collections.Generic;
    using Models;
    using Params;

    public class ProductServiceImpl : ProductService
    {
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
    }
}
