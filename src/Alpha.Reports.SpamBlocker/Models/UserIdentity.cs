namespace Alpha.Reports.SpamBlocker.Models
{
    using Sunplus.Agent.Model.Base;
    using System.Web.Mvc;

    public class UserIdentity : IRequestIdentity
    {
        public string GetKey(ActionExecutingContext filterContext)
        {
            var userSession = new UserSessionExt();
            return $"{filterContext.HttpContext.Request.Path}_{userSession.UserId}";
        }
    }
}
