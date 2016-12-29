namespace Alpha.Reports.Site.Requests.WinLoss
{
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.WinLoss;

    public class EnteredSportResultQuery : IRequest<EnteredResultViewModel>
    {
        public EnteredSportResultQuery() : this(null)
        {
        }

        public EnteredSportResultQuery(IUserSession userSession)
        {
            var session = userSession ?? new UserSessionExt();

            CustId = session.CustId;
            UserLevelId = session.RoleId;

            Language = session.Language;
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte UserLevelId { get; set; }

        public string Language { get; }
    }
}