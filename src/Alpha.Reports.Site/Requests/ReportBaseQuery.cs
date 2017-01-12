namespace Alpha.Reports.Site.Requests
{
    using System;
    using Reports.Models;

    public class ReportBaseQuery
    {
        internal ReportBaseQuery()
        {
        }

        protected ReportBaseQuery(IUser user)
        {
            CustId = user.Id;
            CustName = user.Name;
            CustLevelId = user.LevelId;
            Language = user.Language;
        }

        public string Language { get; }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte CustLevelId { get; set; }

        public string DrillDownInfoJson { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}