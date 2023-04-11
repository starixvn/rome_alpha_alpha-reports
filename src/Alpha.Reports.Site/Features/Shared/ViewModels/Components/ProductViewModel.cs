namespace Alpha.Reports.Site.Features.Shared.ViewModels.Components
{
    using System.Collections.Generic;

    public class ProductViewModel
    {
        public ProductViewModel(int id, string name, bool selected, IEnumerable<ProductViewModel> products)
        {
            Id = id;
            Name = name;
            Checked = selected;
            Products = products;
        }

        public int Id { get; }

        public string Name { get; }

        public bool Checked { get; }

        public IEnumerable<ProductViewModel> Products { get; }
    }
}