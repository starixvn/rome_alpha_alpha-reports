namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using Models;

    public class WinLossParam : SupportedHistoryReportParam
    {
        public WinLossParam(
            int custId,
            string site,
            int currencyId,
            DateTime fromDate,
            DateTime toDate,
            DateTime limitedDate,
            IEnumerable<int> userSelectedProductIds,
            UserLevel reportLevel,
            bool isHistoricReport) : base(fromDate, toDate, limitedDate, isHistoricReport)
        {
            CustId = custId;
            Site = site;
            CurrencyId = currencyId;
            ReportLevel = reportLevel;

            UserSelectedProductIds = userSelectedProductIds;
        }

        public int CustId { get; }

        public string Site { get; }

        public int CurrencyId { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public UserLevel ReportLevel { get; }
    }
}