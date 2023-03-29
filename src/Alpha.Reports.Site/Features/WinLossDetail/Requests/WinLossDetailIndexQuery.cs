namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.Shared.Requests;
    using Alpha.Reports.Site.ViewModels.WinLossDetail;
    using MediatR;

    public class WinLossDetailIndexQuery : ReportDefaultDateRangeNewQuery, IAsyncRequest<WinLossDetailIndexViewModel>
    {
        public WinLossDetailIndexQuery(IUser viewer) : base(viewer)
        {
            IsEnableSabaPromotion = viewer.IsEnableSabaPromotion;
            IsProtectedMode = viewer.IsProtectedMode;
            SubAccId = viewer.SubAccId;
        }

        public bool IsEnableSabaPromotion { get; }

        public bool IsProtectedMode { get; }

        public int SubAccId { get; }
    }
}