namespace Alpha.Reports.Models.Consolidation
{
    using System.Collections.Generic;

    public class ConsolidationExcludeProduct
    {
        public ConsolidationExcludeProduct(IEnumerable<Product> products)
        {
            Products = products;
        }

        public IEnumerable<Product> Products { get; }
    }
}
