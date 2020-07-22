namespace Alpha.Reports.SpamBlocker.Models
{
    using System.Web.Mvc;

    public class SessionIdentity : IRequestIdentity
    {
        public string GetKey(ActionExecutingContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            return $"{httpContext.Request.Path}_{httpContext.Session.SessionID}";
        }
    }
}
