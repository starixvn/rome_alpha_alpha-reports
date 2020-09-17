namespace Alpha.Reports.Models
{
    public class ProductDetail
    {
        public ProductDetail()
        {
        }

        public ProductDetail(
            string refId,
            int resourceId,
            int productId,
            int sportType,
            int betType,
            int order)
        {
            RefId = refId;
            ResourceId = resourceId;
            ProductId = productId;
            SportType = sportType;
            BetType = betType;
            Order = order;
        }

        public string RefId { get; }

        public int ResourceId { get; }

        public int ProductId { get; }

        public int SportType { get; }

        public int BetType { get; }

        public int Order { get; }

        public string DisplayName { get; set; }
    }
}
