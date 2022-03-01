namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models
{
    using System;

    public class BetCreditHistoryModel
    {
        public DateTime ActionDate { get; set; }

        public DateTime? WinLostDate { get; set; }

        public long? RefNo { get; set; }

        public decimal? ActualStake { get; set; }

        public decimal? WinLoss { get; set; }

        public decimal? PlayerComm { get; set; }

        public decimal? BalanceChanged { get; set; }

        public decimal? NewBalance { get; set; }

        public decimal GivenCredit { get; set; }

        public decimal? BetCredit { get; set; }

        public string Action { get; set; }
    }
}
