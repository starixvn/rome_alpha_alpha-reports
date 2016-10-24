namespace Alpha.Reports.Models
{
    public class Product
    {
        public Product(
            string name,
            int id,
            string permission,
            string storeProcedureParameter)
        {
            Name = name;
            Id = id;
            Permission = permission;
            StoreProcedureParameter = storeProcedureParameter;
        }

        public string Name { get; }

        public int Id { get; }

        public string Permission { get; }

        public string StoreProcedureParameter { get; }
    }
}
