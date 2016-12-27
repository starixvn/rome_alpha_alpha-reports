namespace Alpha.Reports.Site.Requests.WinLoss
{
    using Adapters;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.WinLoss;

    public class EnteredResultQuery : IRequest<EnteredResultViewModel>
    {
        public EnteredResultQuery() : this(null)
        {
        }

        public EnteredResultQuery(IUserSession userSession)
        {
            var session = userSession ?? new UserSessionExt();

            if (CustId <= 0)
            {
                CustId = session.CustId;
                UserLevelId = session.RoleId;
            }

            Language = session.Language;
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte UserLevelId { get; set; }

        public string Language { get; }
    }
}