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
            : this(0, username, 0, DateTime.MinValue, DateTime.MinValue, memberWinLoss, memberComm, memberTotal, false)
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
            bool isMultipleMoved)
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

        public int CustId { get; set; }

        public string Username { get; set; }

        public long TransId { get; set; }

        public DateTime OldEventDate { get; set; }

        public DateTime NewEventDate { get; set; }

        public decimal MemberWinLoss { get; set; }

        public decimal MemberComm { get; set; }

        public decimal MemberTotal { get; set; }

        public bool IsMultipleMoved { get; set; }
    }
}
