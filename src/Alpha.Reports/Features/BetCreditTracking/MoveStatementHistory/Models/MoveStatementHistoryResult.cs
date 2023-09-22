namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models
{
    using System.Collections.Generic;

    public class MoveStatementHistoryResult
    {
        public MoveStatementHistoryResult(int errorCode, string errorMess, IEnumerable<MoveStatementHistoryByMember> data, int totalRows)
        {
            ErrorCode = errorCode;
            ErrorMess = errorMess;
            Data = data;
            TotalRows = totalRows;
        }

        public int ErrorCode { get; }

        public string ErrorMess { get; }

        public IEnumerable<MoveStatementHistoryByMember> Data { get; }

        public int TotalRows { get; }
    }
}
