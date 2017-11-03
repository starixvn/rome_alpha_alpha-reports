namespace Alpha.Reports.Models.RacingCommission
{
    using System.Collections.Generic;

    public class RacingCommissionModel
    {
        public RacingCommissionModel()
        {

        }

        public RacingCommissionModel(
            int custId,
            string custName,
            decimal toteWin,
            decimal totePlace,
            decimal toteWinPlace)
        {
            CustId = custId;
            CustName = custName;
            ToteWin = toteWin;
            TotePlace = totePlace;
            ToteWinPlace = toteWinPlace;
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public decimal ToteWin { get; private set; }

        public decimal TotePlace { get; private set; }

        public decimal ToteWinPlace { get; private set; }

        public decimal Total => ToteWin + TotePlace + ToteWinPlace;

        public static RacingCommissionModel GetAccumulatedCommission(IEnumerable<RacingCommissionModel> commissions)
        {
            var accumulatedCommission = new RacingCommissionModel(0, string.Empty, 0, 0, 0);

            foreach (var commission in commissions)
            {
                accumulatedCommission.Add(commission);
            }

            return accumulatedCommission;
        }

        private void Add(RacingCommissionModel commission)
        {
            ToteWin += commission.ToteWin;
            TotePlace += commission.TotePlace;
            ToteWinPlace += commission.ToteWinPlace;
        }
    }
}
