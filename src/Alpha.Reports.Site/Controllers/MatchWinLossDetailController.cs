namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.MatchWinLossDetail;

    public class MatchWinLossDetailController : ReportBaseController
    {
        public MatchWinLossDetailController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult Index(MatchWinLossDetailQuery message) => View(message);

        [HttpGet]
        public ActionResult Excel(MatchWinLossDetailExcelQuery message) => ExcelView(message);

        public ActionResult ReportLegend(ReportLegendQuery message)
            => View("~/Views/ReportLegend/IndexGrossComm.cshtml", message);
    }
}