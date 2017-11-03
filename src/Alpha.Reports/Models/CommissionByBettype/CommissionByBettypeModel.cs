namespace Alpha.Reports.Models.CommissionByBettype
{
    using System.Collections.Generic;

    public class CommissionByBettypeModel
    {
        public CommissionByBettypeModel()
        {

        }

        public CommissionByBettypeModel(
            int custId,
            string custName,
            decimal comm1,
            decimal comm2,
            decimal comm3,
            decimal casinoComm,
            decimal myanmarOddsComm,
            decimal chargeComm,
            decimal others)
        {
            CustId = custId;
            CustName = custName;
            Comm1 = comm1;
            Comm2 = comm2;
            Comm3 = comm3;
            CasinoComm = casinoComm;
            MyanmarOddsComm = myanmarOddsComm;
            ChargeComm = chargeComm;
            Others = others;
        }

        public int CustId { get; }

        public string CustName { get; }

        public decimal Comm1 { get; private set; }

        public decimal Comm2 { get; private set; }

        public decimal Comm3 { get; private set; }

        public decimal CasinoComm { get; private set; }

        public decimal MyanmarOddsComm { get; private set; }

        public decimal ChargeComm { get; private set; }

        public decimal Others { get; private set; }

        public static CommissionByBettypeModel GetAccumulatedCommission(IEnumerable<CommissionByBettypeModel> commissions)
        {
            var accumulatedCommission = new CommissionByBettypeModel(0, string.Empty, 0, 0, 0, 0, 0, 0, 0);

            foreach (var commission in commissions)
            {
                accumulatedCommission.Add(commission);
            }

            return accumulatedCommission;
        }

        private void Add(CommissionByBettypeModel commission)
        {
            Comm1 += commission.Comm1;
            Comm2 += commission.Comm2;
            Comm3 += commission.Comm3;

            CasinoComm += commission.CasinoComm;
            MyanmarOddsComm += commission.MyanmarOddsComm;
            ChargeComm += commission.ChargeComm;
        }
    }
}
