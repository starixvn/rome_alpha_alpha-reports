using System.Web.Mvc;
using Alpha.Reports.Site.Requests.Outstanding;
using MediatR;

namespace Alpha.Reports.Site.Controllers
{
    public class OutstandingController : ReportBaseController
    {
        public OutstandingController(IMediator mediator) : base(mediator)
        {
        }

        [Route("outstanding/super")]
        [Route("outstanding/master")]
        [Route("outstanding/agent")]
        public ActionResult Index(OutstandingQuery message) => View(message);
    }
}