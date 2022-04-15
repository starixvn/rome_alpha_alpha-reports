namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Params
{
    using System;

    public class BetCreditHistoryParam
    {
        public BetCreditHistoryParam(
            string member, 
            long? refNo, 
            DateTime fromDate, 
            DateTime toDate, 
            int viewerId, 
            DateTime limitedDateToShowInfo, 
            int pageIndex, 
            int pageSize)
        {
            Member = member;
            RefNo = refNo;
            FromDate = fromDate;
            ToDate = toDate;
            ViewerId = viewerId;
            LimitedDateToShowInfo = limitedDateToShowInfo;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public string Member { get; }

        public long? RefNo { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int ViewerId { get; }

        public DateTime LimitedDateToShowInfo { get; }

        public int PageIndex { get; }

        public int PageSize { get; }
    }
}
