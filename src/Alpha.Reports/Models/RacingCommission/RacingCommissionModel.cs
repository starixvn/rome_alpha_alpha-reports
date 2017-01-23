using System.Collections.Generic;

namespace Alpha.Reports.Models.RacingCommission
{
    public class RacingCommissionModel
    {
        public int CustId { get; set; }

        public string CustName { get; set; }

        public decimal ToteWin { get; set; }

        public decimal TotePlace { get; set; }

        public decimal ToteWinPlace { get; set; }

        public decimal Total => ToteWin + TotePlace + ToteWinPlace;

        public static RacingCommissionModel GetAccumulatedCommission(IEnumerable<RacingCommissionModel> commissions)
        {
            var accumulatedCommission = new RacingCommissionModel();

            foreach (var commission in commissions)
            {
                accumulatedCommission.ToteWin += commission.ToteWin;
                accumulatedCommission.TotePlace += commission.TotePlace;
                accumulatedCommission.ToteWinPlace += commission.ToteWinPlace;
            }

            return accumulatedCommission;
        }
    }
}
