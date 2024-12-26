namespace Alpha.Reports.Site.Features.WhiteLabelReport.Requests
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.WhiteLabelReport.ViewModels;
    using MediatR;

    public class DailyWinLossQuery : DailyWinLossGetDataQuery, IAsyncRequest<DailyWinLossViewModel>
    {
        public DailyWinLossQuery(IUser viewer) : base(viewer)
        {
        }
    }
}