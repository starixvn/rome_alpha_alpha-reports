namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models
{
    using System;

    public class BetCreditHistoryModel
    {
        public DateTime ActionDateTime { get; set; }

        public DateTime WinLossDate { get; set; }

        public int RefNo { get; set; }

        public decimal ActualStake { get; set; }

        public decimal WinLoss { get; set; }

        public decimal PlayerComm { get; set; }

        public decimal BalanceChange { get; set; }

        public decimal NewBalance { get; set; }

        public decimal GivenCredit { get; set; }

        public decimal BetCredit { get; set; }

        public string Action { get; set; }
    }
}
