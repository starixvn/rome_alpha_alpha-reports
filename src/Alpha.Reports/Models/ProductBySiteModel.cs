namespace Alpha.Reports.Models
{
    public class ProductBySiteModel
    {
        public ProductBySiteModel()
        {
        }

        public ProductBySiteModel(int productId)
        {
            ProductId = productId;
        }

        public int ProductId { get;}
    }
}