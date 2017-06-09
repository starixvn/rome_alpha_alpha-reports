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
            IEnumerable<WinLossBySportModel> winLossBySportModels)
        {
            ProductId = productId;
            ProductName = productName;

            WinLossBySportModels = winLossBySportModels;
            AccumulateTotal();
        }

        public int ProductId { get; }

        public string ProductName { get; }

        public IEnumerable<WinLossBySportModel> WinLossBySportModels { get; }

        public WinLossByBetTypeModel Total { get; private set; }

        public WinLossByProductModel AccumulateTotal()
        {
            Total = new WinLossByBetTypeModel();

            foreach (var winLossBySportModel in WinLossBySportModels)
            {
                Total.Add(winLossBySportModel.Total);
            }

            return this;
        }
    }
}