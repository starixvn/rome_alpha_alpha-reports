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
            IEnumerable<string> userSelectedLeagueIds,
            IEnumerable<int> userSelectedProductIds,
            UserLevel reportLevel)
        {
            CustId = custId;
            FromDate = fromDate == DateTime.MinValue ? SystemTime.Today : fromDate;
            ToDate = toDate == DateTime.MinValue ? SystemTime.Today : toDate;
            UserSelectedLeagueIds = userSelectedLeagueIds;
            UserSelectedProductIds = userSelectedProductIds;
            ReportLevel = reportLevel;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<string> UserSelectedLeagueIds { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public UserLevel ReportLevel { get; }
    }
}
