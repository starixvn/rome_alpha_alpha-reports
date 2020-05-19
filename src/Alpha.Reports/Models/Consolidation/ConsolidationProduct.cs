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

        public byte Id { get; }

        public string Name { get; }

        public bool IsEnable { get; }

        public DateTime? EndOfSupportDate { get; }

        public int Type { get; }

        public int OrderNo { get; }
    }
}
