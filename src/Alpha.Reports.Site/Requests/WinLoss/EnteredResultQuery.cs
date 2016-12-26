namespace Alpha.Reports.Site.Requests.WinLoss
{
    using Helpers;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.WinLoss;

    public class EnteredResultQuery : IRequest<EnteredResultViewModel>
    {
        private readonly IUserSession userSession;

        public EnteredResultQuery() : this(null)
        {
        }

        public EnteredResultQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();

            if (!HttpAdapter.HasContext)
            {
                return;
            }

            if (CustId <= 0)
            {
                CustId = this.userSession.CustId;
                UserLevelId = this.userSession.RoleId;
            }

            Language = this.userSession.Language;
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte UserLevelId { get; set; }

        public string Language { get; }
    }
}