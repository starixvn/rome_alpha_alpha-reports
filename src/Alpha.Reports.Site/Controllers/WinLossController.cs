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

        public ActionResult Index(WinLossQuery message)
            => View(message);

        [HttpGet]
        public ActionResult Excel(WinLossExcelQuery message)
            => ExcelView(message);
    }
}