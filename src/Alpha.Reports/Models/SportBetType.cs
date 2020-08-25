namespace Alpha.Reports.Models
{
    public class SportBetType
    {
        public SportBetType(int productId, int id, string name, bool isSportType)
        {
            ProductId = productId;
            Id = id;
            Name = name;
            IsSportType = isSportType;
        }

        public int ProductId { get; }

        public int Id { get; }

        public string Name { get; }

        public bool IsSportType { get; }

        public string Value => $"{ProductId}_{Id}";

        public bool Selected { get; set; }
    }
}
