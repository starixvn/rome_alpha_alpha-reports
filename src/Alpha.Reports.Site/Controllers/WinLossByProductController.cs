namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using Alpha.Web.Filters;
    using MediatR;
    using Requests.WinLossByProduct;

    [AllowProtectedMode]
    public class WinLossByProductController : ReportBaseController
    {
        public WinLossByProductController(IMediator mediator) : base(mediator)
        {
        }

        [Route("WinLossByProduct/master")]
        [Route("WinLossByProduct/agent")]
        [Route("WinLossByProduct/member")]
        public ActionResult Index(WinLossByProductQuery message)
            => View(message);

        [HttpGet]
        public ActionResult Excel(WinLossByProductExcelQuery message)
          => ExcelView(message);
    }
}