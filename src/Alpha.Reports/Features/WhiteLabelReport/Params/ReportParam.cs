namespace Alpha.Reports.Features.WhiteLabelReport.Params
{
    using System;

    public class ReportParam
    {
        public ReportParam(
            int viewerId,
            DateTime fromDate,
            DateTime toDate,
            string whiteLabels,
            string username,
            string siteName)
        {
            ViewerId = viewerId;
            FromDate = fromDate;
            ToDate = toDate;
            WhiteLabels = whiteLabels;
            Username = username;
            SiteName = siteName;
        }

        public int ViewerId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string WhiteLabels { get; }

        public string Username { get; }

        public string SiteName { get; }
    }
}
