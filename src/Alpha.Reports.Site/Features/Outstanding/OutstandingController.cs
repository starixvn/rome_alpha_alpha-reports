namespace Alpha.Reports.Site.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using MediatR;
    using Requests.Outstanding;

    public class OutstandingController : ReportBaseController
    {
        private readonly string outstandingPath = "~/Features/Outstanding/Views/Index.cshtml";
        private readonly string outstandingNewPath = "~/Features/Outstanding/Views/IndexNew.cshtml";
        private readonly string outstandingNew1Path = "~/Features/Outstanding/Views/IndexNew1.cshtml";
        private readonly string ReportLegendPath = "~/Features/Outstanding/Views/ReportLegend.cshtml";

        public OutstandingController(IMediator mediator) : base(mediator)
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        [Route("outstanding/master")]
        [Route("outstanding/agent")]
        [Route("outstanding/member")]
        public ActionResult Index(OutstandingQuery message) 
            => View(outstandingPath, mediator.Send(message));

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        [Route("outstanding/masternew")]
        [Route("outstanding/agentnew")]
        [Route("outstanding/membernew")]
        public ActionResult IndexNew(OutstandingQuery message) 
            => View(outstandingNewPath, mediator.Send(message));

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        [Route("outstanding/masternew1")]
        [Route("outstanding/agentnew1")]
        [Route("outstanding/membernew1")]
        public ActionResult IndexNew1(OutstandingQuery message)
          => View(outstandingNew1Path, mediator.Send(message));

        public ActionResult ReportLegend(ReportLegendQuery message) 
            => View(ReportLegendPath, mediator.Send(message));
    }
}