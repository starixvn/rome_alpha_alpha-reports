namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using System.Collections.Generic;
    using MediatR;
    using Models.WinlossDetail;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.WinLossDetail;

    public class WinLossDetailQuery<T> :
        BaseReportQuery,
        IRequest<WinLossDetailViewModel<T>> where T : MemberWinLossDetail
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
            }

            Language = this.userSession.Language;
        }

        public bool IsDirectReportViewAction => userSession.CustId == CustId;

        public string Language { get; }

        public IEnumerable<int> UserSelectedProductIds { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; } = true;

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool HideFilter { get; set; }

        public int UserSelectedDateRangeOption { get; set; }
    }
}