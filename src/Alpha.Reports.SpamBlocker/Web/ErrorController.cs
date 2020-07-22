namespace Alpha.Reports.SpamBlocker.Web
{
    using System.Web.Mvc;

    [DisableThrottling]
    public class ErrorController : Controller
    {
        public ActionResult Blocked()
        {
            Response.StatusCode = HttpStatusCodes.TooManyRequests;
            Response.StatusDescription = "Too Many Requests";
            return Content("Too Many Requests");
        }
    }
}
