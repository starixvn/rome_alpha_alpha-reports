namespace Alpha.Reports.Site.Requests
{
    using System;

    public class ReportBaseQuery
    {
        public int? CustId { get; set; }

        public string PageIndex { get; set; }

        public string PageSize { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string ReportPath { get; set; }
    }
}