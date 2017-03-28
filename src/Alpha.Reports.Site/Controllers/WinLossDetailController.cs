namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.WinLossDetail;

    public class WinLossDetailController : ReportBaseController
    {
        public WinLossDetailController(IMediator mediator) : base(mediator)
        {
        }

        [Route("winlossdetail/master")]
        [Route("winlossdetail/agent")]
        [Route("winlossdetail/member")]
        public ActionResult Index(WinLossDetailQuery message)
            => View(message);

        [HttpGet]
        public ActionResult Excel(WinLossDetailExcelQuery message)
            => ExcelView(message);

        public ActionResult ReportLegend(ReportLegendQuery message) => View(message);
    }
}