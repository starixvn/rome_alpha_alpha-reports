namespace Alpha.Reports.Models.CommissionByBettype
{
    using System.Collections.Generic;

    public class CommissionByBettypeModel
    {
        public int CustId { get; set; }

        public int Recommend { get; set; }

        public int Mrecommend { get; set; }

        public string CustName { get; set; }

        public decimal TotalComm1 { get; set; }

        public decimal TotalComm2 { get; set; }

        public decimal TotalComm3 { get; set; }

        public decimal MOComm { get; set; }

        public decimal CasinoComm { get; set; }

        public decimal OtherComm { get; set; }

        public decimal ChargeComm { get; set; }

        public static CommissionByBettypeModel GetAccumulatedCommission(IEnumerable<CommissionByBettypeModel> commissions)
        {
            var accumulatedCommission = new CommissionByBettypeModel();

            foreach (var commission in commissions)
            {
                accumulatedCommission.TotalComm1 += commission.TotalComm1;
                accumulatedCommission.TotalComm2 += commission.TotalComm2;
                accumulatedCommission.TotalComm3 += commission.TotalComm3;

                accumulatedCommission.MOComm += commission.MOComm;
                accumulatedCommission.CasinoComm += commission.CasinoComm;
                accumulatedCommission.OtherComm += commission.OtherComm;
                accumulatedCommission.ChargeComm += commission.ChargeComm;
            }

            return accumulatedCommission;
        }
    }
}
