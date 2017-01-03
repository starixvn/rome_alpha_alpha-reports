namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.WinLoss;

    public class WinLossController : ReportBaseController
    {
        public WinLossController(IMediator mediator) : base(mediator)
        {
        }

        [Route("enteredresult/super")]
        [Route("enteredresult/master")]
        [Route("enteredresult/agent")]
        public ActionResult EnteredResult(EnteredSportResultQuery message)
            => View(message);

        [HttpGet]
        public ActionResult EnteredResultExcel(EnteredSportResultExcelQuery message)
            => ExcelView(message);

        public ActionResult WinLoss(WinLossQuery message)
            => View(message);

        [HttpGet]
        public ActionResult WinLossExcel(WinLossExcelQuery message)
            => ExcelView(message);      
    }
}