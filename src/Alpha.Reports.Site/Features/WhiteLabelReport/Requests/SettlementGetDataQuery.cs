namespace Alpha.Reports.Site.Features.WhiteLabelReport.Requests
{
    using System;
    using System.Web.Mvc;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.WhiteLabelReport.ViewModels;
    using Alpha.Reports.Site.Requests;
    using Alpha.Reports.Utils;
    using MediatR;

    public class SettlementGetDataQuery : ReportBaseQuery, IAsyncRequest<SettlementGetDataViewModel>
    {
        public SettlementGetDataQuery(IUser viewer) : base(viewer)
        {
            MinDate = SystemTime.Today.AddYears(-1).FirstDayOfMonth();
            var lastMonth = SystemTime.Today.AddMonths(-1);
            FromDate = lastMonth.FirstDayOfMonth();
            ToDate = lastMonth.LastDayOfMonth();
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string WhiteLabels { get; set; }

        [AllowHtml]
        public string Username { get; set; }

        public override bool IsValid() => MinDate <= FromDate && ViewerId > 0;
    }
}
