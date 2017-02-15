namespace Alpha.Reports.Models.CommissionByBettype
{
    using System.Collections.Generic;

    public class CommissionByBettypeModel
    {
        public int CustId { get; set; }

        public string CustName { get; set; }

        public decimal Comm1 { get; set; }

        public decimal Comm2 { get; set; }

        public decimal Comm3 { get; set; }

        public decimal CasinoComm { get; set; }

        public decimal MOComm { get; set; }

        public decimal OtherComm { get; set; }

        public decimal ChargeComm { get; set; }

        public static CommissionByBettypeModel GetAccumulatedCommission(IEnumerable<CommissionByBettypeModel> commissions)
        {
            var accumulatedCommission = new CommissionByBettypeModel();

            foreach (var commission in commissions)
            {
                accumulatedCommission.Comm1 += commission.Comm1;
                accumulatedCommission.Comm2 += commission.Comm2;
                accumulatedCommission.Comm3 += commission.Comm3;

                accumulatedCommission.CasinoComm += commission.CasinoComm;
                accumulatedCommission.MOComm += commission.MOComm;
                accumulatedCommission.OtherComm += commission.OtherComm;
                accumulatedCommission.ChargeComm += commission.ChargeComm;
            }

            return accumulatedCommission;
        }
    }
}
