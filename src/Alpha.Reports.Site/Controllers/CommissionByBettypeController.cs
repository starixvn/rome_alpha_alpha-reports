namespace Alpha.Reports.Site.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using MediatR;
    using Requests.CommissionByBettype;

    public class CommissionByBettypeController : ReportBaseController
    {
        public CommissionByBettypeController(IMediator mediator) : base(mediator)
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        [Route("commissionbybettype/master")]
        [Route("commissionbybettype/agent")]
        [Route("commissionbybettype/member")]
        public ActionResult Index(CommissionByBettypeQuery message) => View(message);

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        [HttpGet]
        public ActionResult Excel(CommissionByBettypeExcelQuery message) => ExcelView(message);
    }
}