namespace Alpha.Reports.Site.Models
{
    using Alpha.Reports.Models;
    using Sunplus.Agent.Model.Entities;

    public class UserImpl : User
    {
        private readonly IUserSession userSession;

        public UserImpl(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public int CustId
        {
            get
            {
                return userSession.CustId;
            }
        }

        public string CustName
        {
            get
            {
                return userSession.UserName;
            }
        }

        public string Language
        {
            get
            {
                return userSession.Language;
            }
        }

        public byte UserLevelId
        {
            get
            {
                return userSession.RoleId;
            }
        }
    }
}