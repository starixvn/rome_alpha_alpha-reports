namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using Criteria;
    using Models;

    public interface ProductService
    {
        List<Product> GetProductsByCustomer(GetProductByCustomerCriteria criteria);
    }
}
