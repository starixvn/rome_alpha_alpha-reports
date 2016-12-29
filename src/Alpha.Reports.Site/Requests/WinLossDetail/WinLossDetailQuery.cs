namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.WinLossDetail;

    public class WinLossDetailQuery :
        BaseReportQuery,
        IRequest<WinLossDetailViewModel>
    {
        private readonly IUserSession userSession;

        public WinLossDetailQuery() : this(null)
        {
        }

        public WinLossDetailQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();

            CustId = this.userSession.CustId;
            CustName = this.userSession.UserName;
            UserLevelId = this.userSession.RoleId;

            Language = this.userSession.Language;
        }

        public bool IsDirectReportViewAction => CustId == userSession.CustId;
        
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