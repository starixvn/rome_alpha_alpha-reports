namespace Alpha.Reports.Site.Requests
{
    using System;

    public class ReportBaseQuery
    {
        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte UserLevelId { get; set; }

        public string BreadcrumbLevels { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}