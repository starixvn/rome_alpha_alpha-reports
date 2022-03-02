namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models
{
    using Alpha.Reports.Extensions;
    using System;

    public class BetCreditHistoryModel
    {
        public BetCreditHistoryModel(
            DateTime actionDate, 
            DateTime? winLostDate, 
            long? refNo, 
            decimal? actualStake, 
            decimal? winLoss, 
            decimal? playerComm, 
            decimal? balanceChanged, 
            decimal? newBalance, 
            decimal givenCredit, 
            decimal? betCredit, 
            string action)
        {
            ActionDate = actionDate;
            WinLostDate = winLostDate;
            RefNo = refNo;
            ActualStake = actualStake;
            WinLoss = winLoss;
            PlayerComm = playerComm;
            BalanceChanged = balanceChanged;
            NewBalance = newBalance;
            GivenCredit = givenCredit;
            BetCredit = betCredit;
            Action = action;
        }

        public DateTime ActionDate { get; }

        public DateTime? WinLostDate { get; }

        public long? RefNo { get;  }

        public decimal? ActualStake { get; }

        public decimal? WinLoss { get; }

        public decimal? PlayerComm { get; }

        public decimal? BalanceChanged { get; }

        public decimal? NewBalance { get; }

        public decimal GivenCredit { get; }

        public decimal? BetCredit { get; }

        public string Action { get; }

        public string ActionDateString => ActionDate.ToLongDate24HourWithSlash();

        public string WinLostDateString => WinLostDate.ToShortDateWithSlash();
    }
}
