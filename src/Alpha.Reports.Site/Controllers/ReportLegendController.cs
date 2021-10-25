namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using Alpha.Web.Filters;
    using MediatR;
    using Requests;

    public class ReportLegendController : ReportBaseController
    {
        public ReportLegendController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult Index(ReportLegendQuery message) => View(message);

        [AllowProtectedMode]
        public ActionResult IndexGrossComm(ReportLegendGrossCommQuery message) => View(message);
    }
}