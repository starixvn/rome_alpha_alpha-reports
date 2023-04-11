namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using Alpha.Reports.Constants;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.Shared.Requests;
    using Alpha.Reports.Site.ViewModels.WinLossDetail;
    using MediatR;

    public class WinLossDetailNewQuery : ReportDefaultDateRangeNewQuery, IAsyncRequest<WinLossDetailNewViewModel>
    {
        public WinLossDetailNewQuery(IUser viewer) : base(viewer)
        {
            IsEnableSabaPromotion = viewer.IsEnableSabaPromotion;
            IsProtectedMode = viewer.IsProtectedMode;
            SubAccId = viewer.SubAccId;
        }

        public string ProductIds { get; set; } = DefaultVariables.AllProducts;

        public string ResourceIds { get; set; } = DefaultVariables.AllProductDetails;

        public string BreadcrumbLevelJson { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public bool IsEnableSabaPromotion { get; }

        public bool IsProtectedMode { get; }

        public int SubAccId { get; }
    }
}