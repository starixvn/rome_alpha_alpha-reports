namespace Alpha.Reports.SpamBlocker.Models
{
    using Sunplus.Agent.Model.Base;

    public class UserIdentity : IRequestIdentity
    {
        public string GetKey()
        {
            var userSession = new UserSessionExt();
            return userSession.UserId.ToString();
        }
    }
}
