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
            string firstName,
            string lastName)
        {
            CustId = custId;
            CustName = custName;
            Comm1 = comm1;
            Comm2 = comm2;
            Comm3 = comm3;
            CasinoComm = casinoComm;
            MyanmarOddsComm = myanmarOddsComm;
            FirstName = firstName;
            LastName = lastName;
        }

        public int CustId { get; }

        public string CustName { get; }

        public decimal Comm1 { get; private set; }

        public decimal Comm2 { get; private set; }

        public decimal Comm3 { get; private set; }

        public decimal CasinoComm { get; private set; }

        public decimal MyanmarOddsComm { get; private set; }

        public string FirstName { get; }

        public string LastName { get; }

        public static CommissionByBettypeModel GetAccumulatedCommission(IEnumerable<CommissionByBettypeModel> commissions)
        {
            var accumulatedCommission = new CommissionByBettypeModel(0, string.Empty, 0, 0, 0, 0, 0, null, null);

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
            MyanmarOddsComm += commission.MyanmarOddsComm;

            CasinoComm += commission.CasinoComm;
        }
    }
}
