namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models
{
    using System.Collections.Generic;

    public class BetCreditHistoryResultModel
    {
        public BetCreditHistoryResultModel(int errorCode, string errorMess, IEnumerable<BetCreditHistoryModel> data, int totalRows)
        {
            ErrorCode = errorCode;
            ErrorMess = errorMess;
            Data = data;
            TotalRows = totalRows;
        }

        public int ErrorCode { get; }

        public string ErrorMess { get; }

        public IEnumerable<BetCreditHistoryModel> Data { get; }

        public int TotalRows { get; }
    }
}
