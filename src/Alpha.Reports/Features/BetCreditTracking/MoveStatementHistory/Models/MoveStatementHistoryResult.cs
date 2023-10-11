namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models
{
    using System.Collections.Generic;

    public class MoveStatementHistoryResult
    {
        public MoveStatementHistoryResult(int errorCode, string errorMess, IEnumerable<MoveStatementHistoryByMember> data, MoveStatementHistorySummary summary, int totalRows)
        {
            ErrorCode = errorCode;
            ErrorMess = errorMess;
            Data = data;
            Summary = summary;
            TotalRows = totalRows;
        }

        public int ErrorCode { get; }

        public string ErrorMess { get; }

        public IEnumerable<MoveStatementHistoryByMember> Data { get; }

        public MoveStatementHistorySummary Summary { get; }

        public int TotalRows { get; }
    }
}
