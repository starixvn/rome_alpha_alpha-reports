namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using Alpha.Web.Filters;
    using MediatR;
    using Requests.WinLossDetail;
    using ViewModels.WinLossDetail;

    [AllowProtectedMode]
    public class WinLossDetailController : ReportBaseController
    {
        public WinLossDetailController(IMediator mediator) : base(mediator)
        {
        }

        [Route("winlossdetail/master")]
        [Route("winlossdetail/agent")]
        [Route("winlossdetail/member")]
        public ActionResult Index(WinLossDetailQuery message)
            => View<WinLossDetailViewModel>(message);

        [HttpGet]
        public ActionResult Excel(WinLossDetailExcelQuery message)
            => ExcelView(message);

        public ActionResult ReportLegend(ReportLegendQuery message) => View(message);
    }
}