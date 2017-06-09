using System.Collections.Generic;
using Alpha.Reports.Constants;
using Alpha.Reports.Models.WinlossByBetType;

namespace Alpha.Reports.Models.WinLossByProduct
{
    public class WinLossBySportModel
    {
        public WinLossBySportModel(
            int sportId,
            string sportName,
            IEnumerable<WinLossByBetTypeModel> winLossByBetTypeData)
        {
            SportId = sportId;
            SportName = sportName;

            WinLossByBetTypeData = winLossByBetTypeData;
            AccumulateTotal();
        }

        public int SportId { get; }

        public string SportName { get; }

        public IEnumerable<WinLossByBetTypeModel> WinLossByBetTypeData { get; }

        public WinLossByBetTypeModel Total { get; private set; }

        public static bool IsShowBetTypeDirectly(int productId) => productId != ProductIdConstants.NG2;

        public WinLossBySportModel AccumulateTotal()
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
