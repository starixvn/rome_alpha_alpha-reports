namespace Alpha.Reports.Site.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Reports.Models;

    public class ReportBaseQuery
    {
        internal ReportBaseQuery()
        {
        }

        protected ReportBaseQuery(IUser loginUser)
        {
            // Default data, if query string had data, then get from query string.
            CustId = loginUser.Id;
            CustName = loginUser.Name;
            CustLevelId = loginUser.LevelId;

            Language = loginUser.Language;

            LoginUserId = loginUser.Id;
            LoginUserLevelId = loginUser.LevelId;
        }

        public string Language { get; }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte CustLevelId { get; set; }

        public IEnumerable<CustInfo> BreadcrumbLevels { get; set; } = Enumerable.Empty<CustInfo>();

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int LoginUserId { get; }

        public byte LoginUserLevelId { get; }
    }
}