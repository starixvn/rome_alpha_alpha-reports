namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models
{
    using System.Collections.Generic;

    public class MoveStatementHistoryByMemberModel
    {
        public MoveStatementHistoryByMemberModel(string username, IEnumerable<MoveStatementHistoryByRefNoModel> refNoRow)
        {
            Username = username;
            RefNoRow = refNoRow;
        }

        public string Username { get; }

        public MoveStatementHistoryByRefNoModel MemberRow => new MoveStatementHistoryByRefNoModel(Username);

        public IEnumerable<MoveStatementHistoryByRefNoModel> RefNoRow { get; }
    }
}
