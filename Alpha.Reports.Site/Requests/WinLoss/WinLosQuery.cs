namespace Alpha.Reports.Site.Requests.WinLoss
{
    using System;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.WinLoss;

    public class WinLossQuery :
        BaseReportQuery,
        IRequest<WinLossViewModel>
    {
        private IUserSession userSession;

        public WinLossQuery() : this(null)
        {
        }

        public WinLossQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();

            if (CustId <= 0)
            {
                CustId = this.userSession.CustId;
                CustName = this.userSession.UserName;
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

        public bool HasSubmittedParams
        {
            get
            {
                return FromDate != DateTime.MinValue;
            }
        }

        public string BreadcrumbLevels { get; set; }
    }
}