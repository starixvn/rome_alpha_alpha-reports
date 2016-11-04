namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using MediatR;
    using Models.WinlossDetail;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.WinLossDetail;

    public class WinLossDetailQuery<T> :
        BaseReportQuery,
        IRequest<WinLossDetailViewModel<T>> where T : MemberWinLossDetail
    {
        public WinLossDetailQuery() : this(null)
        {
        }

        public WinLossDetailQuery(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();

            if (IsDirectReportViewAction)
            {
                CustId = userSession.CustId;
            }

            Language = userSession.Language;
        }

        private bool IsDirectReportViewAction => CustId <= 0;

        public string Language { get; }

        public string UserSelectedProductIds { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool HideFilter { get; set; }

        public int UserSelectedDateRangeOption { get; set; }
    }
}