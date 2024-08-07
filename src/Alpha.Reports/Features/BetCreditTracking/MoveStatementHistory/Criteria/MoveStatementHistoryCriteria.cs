namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class MoveStatementHistoryCriteria : CriteriaBase
    {
        public MoveStatementHistoryCriteria(
            string username,
            DateTime fromDate,
            DateTime toDate,
            int loggedId,
            int pageIndex,
            int pageSize,
            bool isVariance)
        {
            Username = username;
            FromDate = fromDate;
            ToDate = toDate;
            LoggedId = loggedId;
            PageIndex = pageIndex;
            PageSize = pageSize;
            IsVariance = isVariance;
        }

        public string Username { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int LoggedId { get; }

        public int ErrorCode { get; set; }

        public string ErrorMess { get; set; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalRows { get; set; }

        public bool IsVariance { get; }

        public override string GetSettingKey() => "Age_Rpt_StatementMovement";

        public override bool IsValid() => FromDate <= ToDate;
    }
}
