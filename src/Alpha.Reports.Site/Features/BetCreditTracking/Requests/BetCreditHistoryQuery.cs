namespace Alpha.Reports.Site.Features.BetCreditTracking.Requests
{
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Features.BetCreditTracking.ViewModels;
    using Alpha.Reports.Site.Requests;
    using Alpha.Reports.Utils;
    using MediatR;
    using System;

    public class BetCreditHistoryQuery : ReportBaseQuery, IRequest<BetCreditHistoryViewModel>
    {
        private readonly DateTime ReleasedDateTime = new DateTime(2022, 7, 27);

        public BetCreditHistoryQuery(IUser viewer) : base(viewer)
        {
            ToDate = SystemTime.Today;
            FromDate = SystemTime.Today;
            MinDate = ReleasedDateTime.GetGreaterDate(ReportDateServiceImpl.Instance.GetBalanceUpDay());
        }

        public string Member { get; set; }

        public long? RefNo { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 100;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MinDate { get; }

        public override bool IsValid()
            => base.IsValid()
            && MinDate <= FromDate
            && FromDate <= ToDate;
    }
}