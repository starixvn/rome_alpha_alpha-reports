namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using Models;

    public interface ProductService
    {
        IEnumerable<Product> GetProductsForCustomer(int custId);
    }
}
