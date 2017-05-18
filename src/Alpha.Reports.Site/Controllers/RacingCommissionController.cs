namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.RacingCommission;

    public class RacingCommissionController : ReportBaseController
    {
        public RacingCommissionController(IMediator mediator) : base(mediator)
        {
        }

        [Route("racingcommission/master")]
        [Route("racingcommission/agent")]
        [Route("racingcommission/member")]
        public ActionResult Index(RacingCommissionQuery message) => View(message);

        [HttpGet]
        public ActionResult Excel(RacingCommissionExcelQuery message) => ExcelView(message);
    }
}