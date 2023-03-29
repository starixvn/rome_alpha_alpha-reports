namespace Alpha.Reports.Site.Features.Shared.ViewModels.Components
{
    using System.Collections.Generic;

    public class ProductViewModel
    {
        public ProductViewModel(int id, string name, IEnumerable<ProductViewModel> products)
        {
            Id = id;
            Name = name;
            Products = products;
        }

        public int Id { get; }

        public string Name { get; }

        public IEnumerable<ProductViewModel> Products { get; }
    }
}