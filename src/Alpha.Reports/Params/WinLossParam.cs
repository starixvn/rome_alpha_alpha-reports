namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using Models;

    public class WinLossParam : SupportedHistoryReportParam
    {
        public WinLossParam(
            int custId,
            int viewerId,
            string site,
            int currencyId,
            DateTime fromDate,
            DateTime toDate,
            DateTime limitedDate,
            IEnumerable<int> userSelectedProductIds,
            IEnumerable<int> userSelectedResourceIds,
            UserLevel reportLevel,
            bool isHistoricReport,
            string productDetailsXML,
            byte levelType) : base(fromDate, toDate, limitedDate, isHistoricReport)
        {
            CustId = custId;
            ViewerId = viewerId;
            Site = site;
            CurrencyId = currencyId;
            ReportLevel = reportLevel;

            UserSelectedProductIds = userSelectedProductIds;
            UserSelectedResourceIds = userSelectedResourceIds;
            ProductDetailsXML = productDetailsXML;
            LevelType = levelType;
        }

        public int CustId { get; }

        public int ViewerId { get; }

        public string Site { get; }

        public int CurrencyId { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public IEnumerable<int> UserSelectedResourceIds { get; }

        public UserLevel ReportLevel { get; }

        public string ProductDetailsXML { get; }

        public byte LevelType { get; }
    }
}