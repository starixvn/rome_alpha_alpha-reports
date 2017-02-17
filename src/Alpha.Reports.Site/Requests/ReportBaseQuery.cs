namespace Alpha.Reports.Site.Requests
{
    using System.Collections.Generic;
    using System.Linq;
    using Enumerations;
    using Reports.Models;

    public class ReportBaseQuery
    {
        internal ReportBaseQuery()
        {
        }

        protected ReportBaseQuery(IUser viewer)
        {
            // Default data, if query string had data, then get from query string.
            CustId = viewer.Id;
            CustName = viewer.Name;
            CustLevelId = viewer.LevelId;

            Language = viewer.Language;

            ViewerId = viewer.Id;
            ViewerLevelId = viewer.LevelId;
            ViewerLevel = Enumeration.FromValue<UserLevel>(ViewerLevelId);
        }

        public string Language { get; }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte CustLevelId { get; set; }

        public virtual UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);

        public int ViewerId { get; }

        public byte ViewerLevelId { get; }

        public virtual UserLevel ViewerLevel { get; }

        public IEnumerable<CustInfo> BreadcrumbLevel { get; set; } = Enumerable.Empty<CustInfo>();

        public virtual bool IsValid()
            => CustId > 0
            && !string.IsNullOrWhiteSpace(CustName)
            && ViewerId > 0
            && ViewerLevel.IsEqualOrUplineOf(CustLevel)
            && !string.IsNullOrWhiteSpace(Language);
    }
}