namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using MediatR;
    using Reports.Models;
    using ViewModels.WinLossDetail;

    public class WinLossDetailQuery :
        ReportDefaultDateRangeQuery,
        IRequest<WinLossDetailViewModel>
    {
        public WinLossDetailQuery(IUser viewer) : base(viewer)
        {
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string UserSelectedProductIds { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public bool IsSetDefaultStackMode => string.IsNullOrWhiteSpace(UserSelectedProductIds);
    }
}