namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.WinLossByBetType;

    public class WinLossByBetTypeController : ReportBaseController
    {
        public WinLossByBetTypeController(IMediator mediator) : base(mediator)
        {
        }

        [Route("WinLossByBettype/master")]
        [Route("WinLossByBettype/agent")]
        [Route("WinLossByBettype/member")]
        public ActionResult Index(WinLossByBetTypeQuery message)
            => View(message);

        [HttpGet]
        public ActionResult Excel(WinLossByBetTypeExcelQuery message)
          => ExcelView(message);
    }
}