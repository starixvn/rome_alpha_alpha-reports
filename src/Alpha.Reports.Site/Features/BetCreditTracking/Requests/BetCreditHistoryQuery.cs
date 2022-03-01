namespace Alpha.Reports.Site.Features.BetCreditTracking.Requests
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.BetCreditTracking.ViewModels;
    using Alpha.Reports.Site.Requests;
    using Alpha.Reports.Utils;
    using MediatR;
    using System;

    public class BetCreditHistoryQuery : ReportBaseQuery, IRequest<BetCreditHistoryViewModel>
    {
        public BetCreditHistoryQuery(IUser viewer) : base(viewer)
        {
        }

        public string Member { get; set; }

        public long? RefNo { get; set; }

        public DateTime FromDate { get; set; } = SystemTime.Today;

        public DateTime ToDate { get; set; } = SystemTime.Today;

        public DateTime MinDate { get; } = SystemTime.Today.AddDays(-14);

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public override bool IsValid()
            => base.IsValid()
            && FromDate <= ToDate && FromDate >= MinDate;
    }
}