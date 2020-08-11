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
            string site,
            int currencyId,
            DateTime fromDate,
            DateTime toDate,
            DateTime limitedDate,
            IEnumerable<string> userSelectedLeagueIds,
            IEnumerable<int> userSelectedProductIds,
            UserLevel reportLevel)
        {
            CustId = custId;
            Site = site;
            CurrencyId = currencyId;
            FromDate = fromDate == DateTime.MinValue ? SystemTime.Today : fromDate;
            ToDate = toDate == DateTime.MinValue ? SystemTime.Today : toDate;
            UserSelectedLeagueIds = userSelectedLeagueIds;
            UserSelectedProductIds = userSelectedProductIds;
            ReportLevel = reportLevel;
            LimitedDate = limitedDate;
        }

        public int CustId { get; }

        public string Site { get; }

        public int CurrencyId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<string> UserSelectedLeagueIds { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public UserLevel ReportLevel { get; }

        public DateTime LimitedDate { get; }
    }
}
