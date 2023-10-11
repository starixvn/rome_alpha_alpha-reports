namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params
{
    using System;

    public class MoveStatementHistoryParam
    {
        public MoveStatementHistoryParam(
            string username, 
            DateTime fromDate, 
            DateTime toDate, 
            int viewerId, 
            DateTime limitedDateToShowInfo,
            int pageIndex,
            int pageSize,
            bool isVariance)
        {
            Username = username;
            FromDate = fromDate;
            ToDate = toDate;
            ViewerId = viewerId;
            LimitedDateToShowInfo = limitedDateToShowInfo;
            PageIndex = pageIndex;
            PageSize = pageSize;
            IsVariance = isVariance;
        }

        public string Username { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int ViewerId { get; }

        public DateTime LimitedDateToShowInfo { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public bool IsVariance { get; }
    }
}
