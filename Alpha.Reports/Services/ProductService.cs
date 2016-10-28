namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using Models;
    using Params;

    public interface ProductService
    {
        List<Product> GetProductsByCustomer(ProductByCustomerParam param);
    }
}
