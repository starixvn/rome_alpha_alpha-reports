namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Requests;
    using Alpha.Reports.Site.ViewModels.WinLossDetail;
    using MediatR;

    public class WinLossDetailIndexQuery : ReportDefaultDateRangeQuery, IAsyncRequest<WinLossDetailIndexViewModel>
    {
        public WinLossDetailIndexQuery(IUser viewer) : base(viewer)
        {
            IsEnableSabaPromotion = viewer.IsEnableSabaPromotion;
        }

        public bool IsEnableSabaPromotion { get; }
    }
}