namespace Alpha.Reports.Models
{
    using System;
    using System.Collections.Generic;

    public class Product
    {
        public Product()
        {
        }

        public Product(int id, string name, DateTime? endOfSupportDate, int type, int orderNo)
        {
            Id = id;
            Name = name;
            EndOfSupportDate = endOfSupportDate;
            Type = type;
            OrderNo = orderNo;
        }

        internal Product(int id, string name, DateTime? endOfSupportDate, int type, int orderNo, Func<int, string> getProductDisplayName)
            : this(id, name, endOfSupportDate, type, orderNo)
        {
            DisplayName = getProductDisplayName == null ? name : getProductDisplayName(id);
        }

        public int Id { get; protected set; }

        public string Name { get; protected set; }

        public DateTime? EndOfSupportDate { get; protected set; }

        public string DisplayName { get; }

        public int Type { get; protected set; }

        public int OrderNo { get; protected set; }

        public IList<Product> SubProducts { get; set; }
    }
}