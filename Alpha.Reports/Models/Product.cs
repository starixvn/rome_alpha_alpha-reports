namespace Alpha.Reports.Models
{
    public class Product
    {
        public Product(
            string name,
            int id,
            string permission,
            bool hasNetTurnoverToolTip)
        {
            Name = name;
            Id = id;
            Permission = permission;
            HasNetTurnoverToolTip = hasNetTurnoverToolTip;
        }

        public string Name { get; }

        public int Id { get; }

        public string Permission { get; }

        public bool HasNetTurnoverToolTip { get; }
    }
}
