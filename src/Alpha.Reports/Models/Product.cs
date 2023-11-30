namespace Alpha.Reports.Models
{
    using System;

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
            TypeName = type == 0 ? string.Empty : (getProductDisplayName == null ? DisplayName : getProductDisplayName(type));
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? EndOfSupportDate { get; set; }

        public string DisplayName { get; }

        public int Type { get; set; }

        public string TypeName { get; }

        public int OrderNo { get; set; }

        public bool IsHide { get; set; }
    }
}