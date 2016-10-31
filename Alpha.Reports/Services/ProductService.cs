namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using Models;
    using Params;

    public interface ProductService
    {
        IEnumerable<Product> GetProductsForCustomer(int custId);
    }
}
