namespace Alpha.Reports.Site.Features.WhiteLabelReport.Requests
{
    using System.Web.Mvc;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.WhiteLabelReport.ViewModels;
    using Alpha.Reports.Site.Requests;
    using MediatR;

    public class DailyWinLossGetDataQuery : ReportDefaultDateRangeQuery, IAsyncRequest<DailyWinLossGetDataViewModel>
    {
        public DailyWinLossGetDataQuery(IUser viewer) : base(viewer)
        {
        }

        public string WhiteLabels { get; set; }

        [AllowHtml]
        public string Username { get; set; }
    }
}