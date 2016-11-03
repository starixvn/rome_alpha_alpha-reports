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

            if (IsNotDrillDownQuery)
            {
                CustId = userSession.CustId;
            }

            Language = userSession.Language;
        }

        private bool IsNotDrillDownQuery => CustId <= 0;

        public string Language { get; }

        public string UserSelectedProductIds { get; set; }

        public string SortedColumn { get; set; }

        public int SortDirection { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool HideFilter { get; set; }

        public int SelectedDateRange { get; set; }
    }
}