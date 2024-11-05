namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models
{
    using System.Collections.Generic;

    public class MoveStatementHistoryByMember
    {
        public MoveStatementHistoryByMember(
            string username,
            MoveStatementHistoryByRefNo memberRow,
            IEnumerable<MoveStatementHistoryByRefNo> refNoRow)
        {
            MemberRow = new MoveStatementHistoryByRefNo(
                username,
                memberRow.MemberWinLoss,
                memberRow.MemberComm,
                memberRow.MemberTotal,
                memberRow.IsDirectMember);
            RefNoRow = refNoRow;
        }

        public MoveStatementHistoryByRefNo MemberRow { get; }

        public IEnumerable<MoveStatementHistoryByRefNo> RefNoRow { get; }
    }
}
