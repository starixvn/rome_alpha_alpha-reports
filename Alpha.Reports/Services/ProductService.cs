namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using Criteria;
    using Models;
    using Params;

    public interface ProductService
    {
        IEnumerable<Product> GetVisibleProducts(GetVisibleProductsParam param);

        List<Product> GetProductsByCustomer(GetProductByCustomerCriteria criteria);      
    }
}
