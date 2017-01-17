namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Utils;

    public class MatchWinLossDetailParam
    {
        public MatchWinLossDetailParam(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            string leagues,
            IEnumerable<int> userSelectedProductIds,
            UserLevel reportLevel)
        {
            CustId = custId;
            FromDate = fromDate == DateTime.MinValue ? SystemTime.Today : fromDate;
            ToDate = toDate == DateTime.MinValue ? SystemTime.Today : toDate;
            Leagues = leagues;
            UserSelectedProductIds = userSelectedProductIds;
            ReportLevel = reportLevel;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string Leagues { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public UserLevel ReportLevel { get; }
    }
}
