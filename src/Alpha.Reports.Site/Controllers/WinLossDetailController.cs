namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.WinLossDetails;

    public class WinLossDetailController : ReportControllerBase
    {
        public WinLossDetailController(IMediator mediator) : base(mediator)
        {
        }

        [Route("winlossdetail/super")]
        [Route("winlossdetail/master")]
        [Route("winlossdetail/agent")]
        public ActionResult Index(WinLossDetailQuery message)
            => View(message);

        [HttpGet]
        public ActionResult Excel(WinLossDetailExcelQuery message)
            => ExcelView(message);
    }
}