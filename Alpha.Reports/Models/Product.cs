namespace Alpha.Reports.Models
{
    using System;

    public class Product
    {
        public Product(int id, string name)
        {
            Id = id;
            Name = name;
        }

        internal Product(int id, string name, Func<int, string> getProductDisplayName) : this(id, name)
        {
            DisplayName = getProductDisplayName == null ? name : getProductDisplayName(id);
        }

        public int Id { get; }

        public string Name { get; }

        public string DisplayName { get; }
    }
}
