namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class BetCreditHistoryCriteria : CriteriaBase
    {
        public BetCreditHistoryCriteria(
            string username,
            DateTime fromDate,
            DateTime toDate,
            long? refNo,
            int loggedId,
            int pageIndex,
            int pageSize)
        {
            Username = username;
            FromDate = fromDate;
            ToDate = toDate;
            RefNo = refNo;
            LoggedId = loggedId;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public string Username { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public long? RefNo { get; }

        public int LoggedId { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalRows { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMess { get; set; }

        public override string GetSettingKey() => "Age_Rpt_BetCreditHistory";

        public override bool IsValid() => FromDate <= ToDate;
    }
}
