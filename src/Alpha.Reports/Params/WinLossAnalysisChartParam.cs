namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using Extensions;
    using Models;
    using Utils;

    public class WinLossAnalysisChartParam
    {
        private const int DefaultPreviousMonths = -2;

        public WinLossAnalysisChartParam(
            int custId,
            string site,
            int currencyId,
            DateTime fromDate,
            DateTime toDate,
            DateTime limitedDate,
            IEnumerable<int> userSelectedProductIds,
            UserLevel custLevel)
        {
            CustId = custId;
            Site = site;
            CurrencyId = currencyId;
            CustLevel = custLevel;
            FromDate = fromDate == DateTime.MinValue ? SystemTime.Today.AddMonths(DefaultPreviousMonths).FirstDayOfMonth() : fromDate.FirstDayOfMonth();
            ToDate = (toDate == DateTime.MinValue || toDate.IsSameMonthWith(SystemTime.Today))
                ? SystemTime.Today : toDate.LastDayOfMonth();
            LimitedDateToShowInfo = limitedDate;
            UserSelectedProductIds = userSelectedProductIds;
        }

        public int CustId { get; }

        public string Site { get; }

        public int CurrencyId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; set; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public UserLevel CustLevel { get; }

        public DateTime LimitedDateToShowInfo { get; internal set; }
    }
}