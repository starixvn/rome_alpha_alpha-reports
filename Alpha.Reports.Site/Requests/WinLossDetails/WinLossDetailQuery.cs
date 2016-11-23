namespace Alpha.Reports.Site.Requests.WinLossDetails
{
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.WinLossDetails;

    public class WinLossDetailQuery :
        BaseReportQuery,
        IRequest<WinLossDetailViewModel>
    {
        private IUserSession userSession;

        public WinLossDetailQuery() : this(null)
        {
        }

        public WinLossDetailQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();

            if (CustId <= 0)
            {
                CustId = this.userSession.CustId;
                UserLevelId = this.userSession.RoleId;
            }

            Language = this.userSession.Language;
        }

        public bool IsDirectReportViewAction
        {
            get
            {
                return CustId == userSession.CustId;
            }
        }

        public string Language { get; }

        public string UserSelectedProductIds { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; } = true;

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool IsFilterVisible { get; set; }

        public int UserSelectedDateRangeOption { get; set; }

        public byte UserLevelId { get; set; }
    }
}