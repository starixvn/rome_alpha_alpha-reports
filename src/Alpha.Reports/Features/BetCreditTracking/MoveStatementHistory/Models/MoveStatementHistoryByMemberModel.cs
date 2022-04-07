namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models
{
    using System.Collections.Generic;

    public class MoveStatementHistoryByMemberModel
    {
        public MoveStatementHistoryByMemberModel(int custId, string username, IEnumerable<MoveStatementHistoryByRefNoModel> refNoRow)
        {
            CustId = custId;
            Username = username;
            RefNoRow = refNoRow;
        }

        public int CustId { get; }

        public string Username { get; }

        public MoveStatementHistoryByRefNoModel MemberRow => new MoveStatementHistoryByRefNoModel(CustId, Username);

        public IEnumerable<MoveStatementHistoryByRefNoModel> RefNoRow { get; }
    }
}
