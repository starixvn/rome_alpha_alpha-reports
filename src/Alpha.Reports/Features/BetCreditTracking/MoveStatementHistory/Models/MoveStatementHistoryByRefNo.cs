namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models
{
    using System;

    public class MoveStatementHistoryByRefNo
    {
        public MoveStatementHistoryByRefNo()
        {
        }

        public MoveStatementHistoryByRefNo(
            string username,
            decimal memberWinLoss,
            decimal memberComm,
            decimal memberTotal)
            : this(0, username, 0, DateTime.MinValue, DateTime.MinValue, memberWinLoss, memberComm, memberTotal, 0)
        {
        }

        public MoveStatementHistoryByRefNo(
            int custId, 
            string username, 
            long transId, 
            DateTime oldEventDate, 
            DateTime newEventDate, 
            decimal memberWinLoss, 
            decimal memberComm, 
            decimal memberTotal, 
            int isMultipleMoved)
        {
            Username = username;
            CustId = custId;
            TransId = transId;
            OldEventDate = oldEventDate;
            NewEventDate = newEventDate;
            MemberWinLoss = memberWinLoss;
            MemberComm = memberComm;
            MemberTotal = memberTotal;
            IsMultipleMoved = isMultipleMoved;
        }

        public int CustId { get;  }

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
