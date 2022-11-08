namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Utils;

    public class WinLossAnalysisParam
    {
        public WinLossAnalysisParam(
          int custId,
          int viewerId,
          string site,
          int currencyId,
          int reportType,
          DateTime fromDate,
          DateTime toDate,
          DateTime limitedDate,
          IEnumerable<int> userSelectedProductIds,
          UserLevel custLevel)
        {
            CustId = custId;
            ViewerId = viewerId;
            Site = site;
            CurrencyId = currencyId;
            CustLevel = custLevel;

            ReportTypeId = reportType;

            FromDate = fromDate == DateTime.MinValue ? SystemTime.Today : fromDate;
            ToDate = toDate == DateTime.MinValue ? SystemTime.Today : toDate;
            LimitedDateToShowInfo = limitedDate;
            UserSelectedProductIds = userSelectedProductIds;
        }

        public int CustId { get; }

        public int ViewerId { get; }

        public string Site { get; }

        public int CurrencyId { get; }

        public int ReportTypeId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public UserLevel CustLevel { get; }

        public DateTime LimitedDateToShowInfo { get; }
    }
}