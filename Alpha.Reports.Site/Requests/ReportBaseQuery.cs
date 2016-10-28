namespace Alpha.Reports.Site.Requests
{
    public class ReportBaseQuery
    {
        public string CustId { get; set; }

        public string PageIndex { get; set; }

        public string PageSize { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string ReportPath { get; set; }
    }
}