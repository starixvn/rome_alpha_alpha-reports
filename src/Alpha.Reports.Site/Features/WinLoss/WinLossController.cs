namespace Alpha.Reports.Site.Controllers
{
    using MediatR;
    using Requests.WinLoss;
    using System.Web.Mvc;

    public class WinLossController : ReportBaseController
    {
        private readonly string winLossPath = "~/Features/WinLoss/Views/Index.cshtml";
        private readonly string winLossNewPath = "~/Features/WinLoss/Views/IndexNew.cshtml";

        public WinLossController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult Index(WinLossQuery message)
            => View(winLossPath, message);

        public ActionResult IndexNew(WinLossQuery message)
            => View(winLossNewPath, message);

        [HttpGet]
        public ActionResult Excel(WinLossExcelQuery message)
            => ExcelView(message);
    }
}