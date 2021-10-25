namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using Alpha.Web.Filters;
    using MediatR;
    using Requests.CommissionByBettype;

    [AllowProtectedMode]
    public class CommissionByBettypeController : ReportBaseController
    {
        public CommissionByBettypeController(IMediator mediator) : base(mediator)
        {
        }

        [Route("commissionbybettype/master")]
        [Route("commissionbybettype/agent")]
        [Route("commissionbybettype/member")]
        public ActionResult Index(CommissionByBettypeQuery message) => View(message);

        [HttpGet]
        public ActionResult Excel(CommissionByBettypeExcelQuery message) => ExcelView(message);

        public ActionResult Guidelines(CommissionByBetTypeGuideQuery message) => View(message);
    }
}