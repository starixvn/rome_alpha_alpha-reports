namespace Alpha.Reports.Site.Requests.WinLoss
{
    using System;
    using Helpers;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.WinLoss;

    public class WinLossQuery :
         BaseReportQuery,
         IRequest<WinLossViewModel>
    {
        private readonly IUserSession userSession;

        public WinLossQuery() : this(null)
        {
        }

        public WinLossQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();

            if (!HttpAdapter.HasContext)
            {
                return;
            }

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

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool IsFilterVisible { get; set; }

        public int UserSelectedDateRangeOption { get; set; }
    }
}