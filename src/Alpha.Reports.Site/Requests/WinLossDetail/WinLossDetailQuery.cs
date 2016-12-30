namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using MediatR;
    using Reports.Models;
    using ViewModels.WinLossDetail;

    public class WinLossDetailQuery :
        ReportBaseQuery,
        IRequest<WinLossDetailViewModel>
    {
        public WinLossDetailQuery(IUser user)
        {
            CustId = user.CustId;
            CustName = user.CustName;
            UserLevelId = user.UserLevelId;
            Language = user.Language;
        }

        public string Language { get; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string UserSelectedProductIds { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; } = true;

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public bool HasSubmittedParams => !string.IsNullOrWhiteSpace(UserSelectedProductIds);
    }
}