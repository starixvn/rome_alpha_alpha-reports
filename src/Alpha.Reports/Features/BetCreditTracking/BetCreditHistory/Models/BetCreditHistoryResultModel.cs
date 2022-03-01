namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models
{
    using System.Collections.Generic;

    public class BetCreditHistoryResultModel
    {
        public int ErrorCode { get; set; }

        public string ErrorMess { get; set; } = "Successfull";

        public IList<BetCreditHistoryModel> Data { get; set; } = new List<BetCreditHistoryModel>();
    }
}
