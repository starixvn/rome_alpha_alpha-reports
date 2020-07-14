namespace Alpha.Reports.SpamBlocker.Models
{
    using Alpha.Reports.SpamBlocker.Web;
    using System.Web.Mvc;

    public class IpIdentity : IRequestIdentity
    {
        public string GetKey(ActionExecutingContext filterContext)
        {
            return filterContext.HttpContext.Request.GetClientIp().ToString();
        }
    }
}
