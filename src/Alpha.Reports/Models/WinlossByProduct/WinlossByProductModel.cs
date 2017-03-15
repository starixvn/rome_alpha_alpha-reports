namespace Alpha.Reports.Models.WinLossByProduct
{
    using System.Collections.Generic;
    using WinlossByBetType;

    public class WinLossByProductModel
    {
        public WinLossByProductModel()
        {
        }

        public WinLossByProductModel(
            int productId,
            string productName,
            IEnumerable<WinLossByBetTypeModel> winLossByBetTypeData)
        {
            ProductId = productId;
            ProductName = productName;

            WinLossByBetTypeData = winLossByBetTypeData;
            AccumulateTotal();
        }

        public int ProductId { get; }

        public string ProductName { get; }

        public IEnumerable<WinLossByBetTypeModel> WinLossByBetTypeData { get; }

        public WinLossByBetTypeModel Total { get; private set; }

        public WinLossByProductModel AccumulateTotal()
        {
            Total = new WinLossByBetTypeModel();

            foreach (var winLossByBetTypeModel in WinLossByBetTypeData)
            {
                Total.Add(winLossByBetTypeModel);
            }

            return this;
        }
    }
}