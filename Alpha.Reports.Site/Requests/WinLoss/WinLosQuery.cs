namespace Alpha.Reports.Site.Requests.WinLoss
{
    using System;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.WinLoss;

    public class WinLossQuery : IRequest<WinLossViewModel>
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

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte UserLevelId { get; set; }

        public string BreadcrumbLevels { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Language { get; }

        public string UserSelectedProductIds { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool IsFilterVisible { get; set; }

        public int UserSelectedDateRangeOption { get; set; }
    }
}