namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.WinLossByBettype;

    public class WinLossByBettypeController : ReportBaseController
    {
        public WinLossByBettypeController(IMediator mediator) : base(mediator)
        {
        }

        [Route("WinLossByBettype/master")]
        [Route("WinLossByBettype/agent")]
        [Route("WinLossByBettype/member")]
        public ActionResult Index(WinLossByBettypeQuery message)
            => View(message);

        [HttpGet]
        public ActionResult Excel(WinLossByBettypeExcelQuery message)
          => ExcelView(message);
    }
}