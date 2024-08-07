namespace Alpha.Reports.Models.Consolidation
{
    using System;

    public class ConsolidationProduct
    {
        public ConsolidationProduct()
        {
        }

        public ConsolidationProduct(byte id, string name, DateTime? endOfSupportDate, bool isEnable, int type, int orderNo)
        {
            Id = id;
            Name = name;
            IsEnable = isEnable;
            EndOfSupportDate = endOfSupportDate;
            Type = type;
            OrderNo = orderNo;
        }

        public byte Id { get; set; }

        public string Name { get; set; }

        public bool IsEnable { get; set; }

        public DateTime? EndOfSupportDate { get; set; }

        public int Type { get; set; }

        public int OrderNo { get; set; }
    }
}
