namespace Alpha.Reports.Site.Models
{
    using Reports.Models;
    using Sunplus.Agent.Model.Entities;

    public class UserImpl : User
    {
        private readonly IUserSession userSession;

        public UserImpl(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public int CustId => userSession.CustId;

        public string CustName => userSession.UserName;

        public string Language => userSession.Language;

        public byte UserLevelId => userSession.RoleId;
    }
}