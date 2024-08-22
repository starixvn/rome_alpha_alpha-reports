namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models
{
    using Alpha.Reports.Extensions;
    using System;

    public class BetCreditHistoryModel
    {
        public BetCreditHistoryModel()
        {
        }

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
            int actionId,
            string action,
            bool isUserLogActionId)
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
            ActionId = actionId;
            Action = action;
            IsUserLogActionId = isUserLogActionId;
        }

        public DateTime ActionDate { get; set; }

        public DateTime? WinLostDate { get; set; }

        public long? RefNo { get; set; }

        public string RefNoString => RefNo.HasValue ? RefNo.ToString() : string.Empty;

        public decimal? ActualStake { get; set; }

        public decimal? WinLoss { get; set; }

        public decimal? PlayerComm { get; set; }

        public decimal? BalanceChanged { get; set; }

        public decimal? NewBalance { get; set; }

        public decimal GivenCredit { get; set; }

        public decimal? BetCredit { get; set; }

        public string Action { get; set; }

        public int ActionId { get; set; }

        public bool IsUserLogActionId { get; set; }

        public string ActionDateString => ActionDate.ToLongDate24HourWithSlash();

        public string WinLostDateString => WinLostDate.ToShortDateWithSlash();
    }
}
