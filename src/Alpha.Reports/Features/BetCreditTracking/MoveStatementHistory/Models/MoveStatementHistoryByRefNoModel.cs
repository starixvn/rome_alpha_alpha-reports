namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models
{
    using System;

    public class MoveStatementHistoryByRefNoModel
    {
        public MoveStatementHistoryByRefNoModel()
        {
        }

        public MoveStatementHistoryByRefNoModel(string username)
        {
            Username = username;
        }

        public string Username { get; }

        public long TransId { get; }

        public DateTime OldEventDate { get; }

        public DateTime NewEventDate { get; }

        public decimal MemberWinLoss { get; }

        public decimal MemberComm { get; }

        public decimal MemberTotal { get; }

        public int IsMultipleMoved { get; }
    }
}
