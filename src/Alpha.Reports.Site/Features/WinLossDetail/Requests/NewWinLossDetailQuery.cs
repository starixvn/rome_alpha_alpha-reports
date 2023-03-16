namespace Alpha.Reports.Site.Features.WinLossDetail.Requests
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.WinLossDetail.ViewModels;
    using Alpha.Reports.Site.Requests;
    using MediatR;

    public class NewWinLossDetailQuery : ReportDefaultDateRangeQuery, IAsyncRequest<NewWinLossDetailViewModel>
    {
        public NewWinLossDetailQuery(IUser viewer) : base(viewer)
        {
            IsEnableSabaPromotion = viewer.IsEnableSabaPromotion;
        }

        public bool IsEnableSabaPromotion { get; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}