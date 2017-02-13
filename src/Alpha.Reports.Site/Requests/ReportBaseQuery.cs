namespace Alpha.Reports.Site.Requests
{
    using System;
    using Reports.Models;
    using Services;
    using Utils;

    public class FromBalanceUpDateDateRangeQuery : ReportBaseQuery
    {
        internal FromBalanceUpDateDateRangeQuery(IUser viewer) : base(viewer)
        {
            ToDate = SystemTime.Today;
            FromDate = SystemTime.Today;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MinDate => ReportDateServiceImpl.Instance.GetBalanceUpDay();

        public override bool IsValid()
        {
            if (FromDate == DateTime.MinValue)
            {
                FromDate = SystemTime.Today;
            }

            return base.IsValid()
                && MinDate <= FromDate
                && FromDate <= ToDate;
        }
    }
}