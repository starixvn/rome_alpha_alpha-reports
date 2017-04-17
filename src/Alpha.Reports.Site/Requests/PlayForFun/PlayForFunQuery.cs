namespace Alpha.Reports.Site.Requests.PlayForFun
{
    using System;
    using Reports.Constants;
    using Reports.Models;
    using Utils;

    public class PlayForFunQuery : ReportBaseQuery
    {
        public PlayForFunQuery(IUser viewer) : base(viewer)
        {
            ToDate = SystemTime.Today;
            FromDate = SystemTime.Today;
        }

        public int UserSelectedProductId { get; set; }

        public int UserSelectedStatusId { get; set; } = StatusIdConstants.All;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MaxDate => SystemTime.Today;

        public DateTime MinDate => MaxDate.AddMonths(-1);

        public int UserSelectedDateRangeOption { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; } = true;

        public bool IsFilterVisible { get; set; } = true;

        public override bool IsValid()
        {
            return base.IsValid()
                && MinDate <= FromDate
                && FromDate <= ToDate
                && ToDate <= MaxDate;
        }
    }
}