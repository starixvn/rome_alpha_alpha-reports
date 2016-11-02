namespace Alpha.Reports.Site.Requests
{
    using System;

    public class BaseReportQuery
    {
        public int CustId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}