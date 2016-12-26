namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Web;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BaseReportController : BaseController
    {
    }
}