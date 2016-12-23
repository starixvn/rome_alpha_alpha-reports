namespace Alpha.Reports.Site.Requests.WinLoss
{
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.WinLoss;

    public class EnterResultQuery : IRequest<EnteredResultViewModel>
    {
        private readonly IUserSession userSession;

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

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte UserLevelId { get; set; }

        public string Language { get; }
    }
}