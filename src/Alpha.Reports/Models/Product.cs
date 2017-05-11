namespace Alpha.Reports.Models
{
    using System;

    public class Product
    {
        public Product(int id, string name, DateTime? endOfSupportDate = null)
        {
            Id = id;
            Name = name;
            EndOfSupportDate = endOfSupportDate;
        }

        internal Product(int id, string name, DateTime? endOfSupportDate, Func<int, string> getProductDisplayName) : this(id, name, endOfSupportDate)
        {
            DisplayName = getProductDisplayName == null ? name : getProductDisplayName(id);
        }

        public int Id { get; }

        public string Name { get; }

        public DateTime? EndOfSupportDate { get; }

        public string DisplayName { get; }
    }
}