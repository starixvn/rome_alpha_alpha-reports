namespace Alpha.Reports.Site.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using MediatR;
    using Requests.Outstanding;

    public class OutstandingController : ReportBaseController
    {
        public OutstandingController(IMediator mediator) : base(mediator)
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        [Route("outstanding/master")]
        [Route("outstanding/agent")]
        [Route("outstanding/member")]
        public ActionResult Index(OutstandingQuery message) => View(message);
    }
}