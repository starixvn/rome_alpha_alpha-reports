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
            decimal casinoComm)
        {
            CustId = custId;
            CustName = custName;
            Comm1 = comm1;
            Comm2 = comm2;
            Comm3 = comm3;
            CasinoComm = casinoComm;
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public decimal Comm1 { get; set; }

        public decimal Comm2 { get; set; }

        public decimal Comm3 { get; set; }

        public decimal CasinoComm { get; set; }

        public static CommissionByBettypeModel GetAccumulatedCommission(IEnumerable<CommissionByBettypeModel> commissions)
        {
            var accumulatedCommission = new CommissionByBettypeModel(0, string.Empty, 0, 0, 0, 0);

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
        }
    }
}