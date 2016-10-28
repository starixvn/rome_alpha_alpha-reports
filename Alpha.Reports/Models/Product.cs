namespace Alpha.Reports.Models
{
    public class Product
    {
        public Product(
            int id,
            string name,
            string permission)
        {
            Id = id;
            Name = name;
            Permission = permission;
        }

        public int Id { get; }

        public string Name { get; }

        public string Permission { get; }
    }
}
