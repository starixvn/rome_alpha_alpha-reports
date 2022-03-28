namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models
{
    using System.Collections.Generic;

    public class MoveStatementHistoryResultModel
    {
        public MoveStatementHistoryResultModel(int errorCode, string errorMess, IEnumerable<MoveStatementHistoryByMemberModel> data)
        {
            ErrorCode = errorCode;
            ErrorMess = errorMess;
            Data = data;
        }

        public int ErrorCode { get; }

        public string ErrorMess { get; }

        public IEnumerable<MoveStatementHistoryByMemberModel> Data { get; }
    }
}
