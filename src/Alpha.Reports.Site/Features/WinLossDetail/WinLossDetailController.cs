namespace Alpha.Reports.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Web.Filters;
    using MediatR;
    using Requests.WinLossDetail;
    using ViewModels.WinLossDetail;

    [AllowProtectedMode]
    public class WinLossDetailController : ReportBaseController
    {
        private const string IndexPath = "~/Features/WinLossDetail/Views/Index.cshtml";
        private const string IndexNewPath = "~/Features/WinLossDetail/Views/IndexNew.cshtml";
        private const string ReportLegendPath = "~/Features/WinLossDetail/Views/ReportLegend.cshtml";
        private const string ReportLegendNewPath = "~/Features/WinLossDetail/Views/ReportLegendNew.cshtml";

        public WinLossDetailController(IMediator mediator) : base(mediator)
        {
        }

        [Route("winlossdetail/master")]
        [Route("winlossdetail/agent")]
        [Route("winlossdetail/member")]
        public ActionResult Index(WinLossDetailQuery message)
            => View<WinLossDetailViewModel>(IndexPath, message);

        [HttpGet]
        public ActionResult Excel(WinLossDetailExcelQuery message)
            => ExcelView(message);

        public ActionResult ReportLegend(ReportLegendQuery message)
            => View(ReportLegendPath, message);

        [Route("winlossdetail/masternew")]
        [Route("winlossdetail/agentnew")]
        [Route("winlossdetail/membernew")]
        public Task<ActionResult> IndexNew(WinLossDetailIndexQuery message)
            => ViewAsync(IndexNewPath, message);

        public Task<ActionResult> GetData(WinLossDetailNewQuery message)
            => JsonAsync<WinLossDetailNewViewModel>(message);

        public Task<FileContentResult> ExcelNew(WinLossDetailExcelNewQuery message)
            => ExcelViewAsync(message);

        public Task<ActionResult> ReportLegendNew(ReportLegendNewQuery message)
            => ViewAsync(ReportLegendNewPath, message);
    }
}