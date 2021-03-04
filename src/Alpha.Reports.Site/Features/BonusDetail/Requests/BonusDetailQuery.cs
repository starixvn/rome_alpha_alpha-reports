namespace Alpha.Reports.Site.Features.BonusDetail.Requests
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Alpha.Reports.Configurations;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.BonusDetail.ViewModels;
    using Alpha.Reports.Site.Requests;
    using Alpha.Reports.Utils;
    using MediatR;

    public class BonusDetailQuery : ReportBaseQuery, IAsyncRequest<BonusDetailViewModel>
    {
        private const int DefaultMinYear = 2021;
        private const int DefaultPageSize = 500;
        private const int SinceLastMonth = 6;

        public BonusDetailQuery(IUser viewer) : base(viewer)
        {
            var lastMonth = SystemTime.Today.AddMonths(-1);
            FromDate = new DateTime(lastMonth.Year, lastMonth.Month, 1);

            if (FromDate < MinDate)
            {
                FromDate = MinDate;
            }
        }

        [AllowHtml]
        public string Member { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; } = SystemTime.Today;

        public DateTime MinDate { get; } = new DateTime(DefaultMinYear, 1, 1);

        public byte? StatusId { get; set; }

        public bool? IsComplained { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = DefaultPageSize;

        public int UserSelectedDateRangeOption { get; set; } = SinceLastMonth;

        public bool IsFilterVisible { get; set; } = true;

        public override bool IsValid()
            => base.IsValid()
            && FromDate <= ToDate && FromDate >= MinDate
            && SiteSettings.AllowedSitesToViewBonusDetail.Contains(Site.ToUpperInvariant());
    }
}