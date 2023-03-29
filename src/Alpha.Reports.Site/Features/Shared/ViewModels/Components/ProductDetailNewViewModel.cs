namespace Alpha.Reports.Site.Features.Shared.ViewModels.Components
{
    public class ProductDetailNewViewModel
    {
        public ProductDetailNewViewModel(int productId, int id, string name)
        {
            ProductId = productId;
            Id = id;
            Name = name;
        }

        public int ProductId { get; }

        public int Id { get; }

        public string Name { get; }
    }
}