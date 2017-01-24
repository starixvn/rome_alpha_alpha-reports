namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.WinLossByGameType;

    public class WinLossByGameTypeController : ReportBaseController
    {
        public WinLossByGameTypeController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult Index(WinLossByGameTypeQuery message)
            => View(message);

        [HttpGet]
        public ActionResult Excel(WinLossByGameTypeExcelQuery message)
          => ExcelView(message);
    }
}