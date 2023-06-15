namespace Alpha.Reports.Site.Features.Shared.ViewModels.Components
{
    public class ProductDetailNewViewModel
    {
        public ProductDetailNewViewModel(int productId, int id, string name, bool selected)
        {
            ProductId = productId;
            Id = id;
            Name = name;
            Checked = selected;
        }

        public int ProductId { get; }

        public int Id { get; }

        public string Name { get; }

        public bool Checked { get; }
    }
}