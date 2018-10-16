namespace Alpha.Reports.Models.Consolidation
{
    using System.Collections.Generic;

    public class ConsolidationExcludeProduct
    {
        public ConsolidationExcludeProduct(IEnumerable<Product> products, IEnumerable<Product> excludeProducts)
        {
            Products = products;
            ExcludeProducts = excludeProducts;
        }

        public IEnumerable<Product> Products { get; }

        public IEnumerable<Product> ExcludeProducts { get; }
    }
}
