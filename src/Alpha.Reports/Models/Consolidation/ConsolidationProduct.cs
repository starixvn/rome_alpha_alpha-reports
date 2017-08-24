using System;

namespace Alpha.Reports.Models.Consolidation
{
    public class ConsolidationProduct
    {
        public ConsolidationProduct(byte id, string name, DateTime? endOfSupportDate, bool isEnable)
        {
            Id = id;
            Name = name;
            IsEnable = isEnable;
            EndOfSupportDate = endOfSupportDate;
        }

        public byte Id { get; }

        public string Name { get; }

        public bool IsEnable { get; }

        public DateTime? EndOfSupportDate { get; }
    }
}
