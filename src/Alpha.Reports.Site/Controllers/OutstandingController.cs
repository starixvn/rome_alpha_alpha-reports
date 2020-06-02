namespace Alpha.Reports.Site.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using MediatR;
    using Requests.Outstanding;

    public class OutstandingController : ReportBaseController
    {
        private readonly string outstandingNewPath = "~/Views/Outstanding/IndexNew.cshtml";

        public OutstandingController(IMediator mediator) : base(mediator)
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        [Route("outstanding/master")]
        [Route("outstanding/agent")]
        [Route("outstanding/member")]
        public ActionResult Index(OutstandingQuery message) => View(message);

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        [Route("outstanding/masternew")]
        [Route("outstanding/agentnew")]
        [Route("outstanding/membernew")]
        public ActionResult IndexNew(OutstandingQuery message) 
            => View(outstandingNewPath, mediator.Send(message));

        public ActionResult ReportLegend(ReportLegendQuery message) => View(message);
    }
}