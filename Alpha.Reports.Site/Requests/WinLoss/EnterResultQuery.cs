namespace Alpha.Reports.Site.Requests.WinLoss
{
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.WinLoss;

    public class EnterResultQuery :
        BaseReportQuery,
        IRequest<EnteredResultViewModel>
    {
        private IUserSession userSession;

        public EnterResultQuery() : this(null)
        {
        }

        public EnterResultQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();

            if (CustId <= 0)
            {
                CustId = this.userSession.CustId;
                UserLevelId = this.userSession.RoleId;
            }

            Language = this.userSession.Language;
        }

        public string Language { get; }
    }
}