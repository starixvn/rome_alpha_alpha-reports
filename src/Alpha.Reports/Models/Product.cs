namespace Alpha.Reports.Models
{
    using System;

    public class Product
    {
        public Product()
        {
        }

        public Product(int id, string name, DateTime? endOfSupportDate)
        {
            Id = id;
            Name = name;
            EndOfSupportDate = endOfSupportDate;
        }

        internal Product(int id, string name, DateTime? endOfSupportDate, Func<int, string> getProductDisplayName) : this(id, name, endOfSupportDate)
        {
            DisplayName = getProductDisplayName == null ? name : getProductDisplayName(id);
        }

        public int Id { get; protected set; }

        public string Name { get; protected set; }

        public DateTime? EndOfSupportDate { get; protected set; }

        public string DisplayName { get; }
    }
}