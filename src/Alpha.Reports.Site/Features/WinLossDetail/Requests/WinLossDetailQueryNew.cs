namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels.WinLossDetail;
    using MediatR;

    public class WinLossDetailQueryNew : WinLossDetailIndexQuery, IAsyncRequest<WinLossDetailViewModelNew>
    {
        public WinLossDetailQueryNew(IUser viewer) : base(viewer)
        {
            IsProtectedMode = viewer.IsProtectedMode;
        }

        public string UserSelectedProductIds { get; set; }

        public string UserSelectedResourceIds { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public bool IsProtectedMode { get; }
    }
}