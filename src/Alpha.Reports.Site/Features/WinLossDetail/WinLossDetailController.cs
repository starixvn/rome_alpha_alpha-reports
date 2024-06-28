namespace Alpha.Reports.Site.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Reports.Site.Features.Shared.ViewModels.Components;
    using Alpha.Web.Filters;
    using MediatR;
    using Requests.WinLossDetail;
    using ViewModels.WinLossDetail;

    [AllowProtectedMode]
    public class WinLossDetailController : ReportBaseController
    {
        private const string BasePath = "~/Features/WinLossDetail/Views";

        public WinLossDetailController(IMediator mediator) : base(mediator)
        {
        }

        [Route("winlossdetail/master")]
        [Route("winlossdetail/agent")]
        [Route("winlossdetail/member")]
        public ActionResult Index(WinLossDetailQuery message)
            => View<WinLossDetailViewModel>($"{BasePath}/{nameof(Index)}.cshtml", message);

        [HttpGet]
        public ActionResult Excel(WinLossDetailExcelQuery message)
            => ExcelView(message);

        public ActionResult ReportLegend(ReportLegendQuery message)
            => View($"{BasePath}/{nameof(ReportLegend)}.cshtml", message);

        [Route("winlossdetail/masternew")]
        [Route("winlossdetail/agentnew")]
        [Route("winlossdetail/membernew")]
        public Task<ActionResult> IndexNew(WinLossDetailNewQuery message)
            => ViewAsync<WinLossDetailNewViewModel>($"{BasePath}/{nameof(IndexNew)}.cshtml", message);

        public Task<ActionResult> GetData(WinLossDetailGetDataQuery message)
            => ViewAsync<WinLossDetailGetDataViewModel>($"{BasePath}/ReportDataNew.cshtml", message);

        public Task<ActionResult> GetDataWeek(WinLossDetailGetDataWeekQuery message)
            => ViewAsync($"{BasePath}/ReportDataWeek.cshtml", message);

        public Task<ActionResult> GetWeeks(WinLossDetailWeeksQuery message)
            => ViewAsync<IEnumerable<WeekSelectionViewModel>>($"{BasePath}/Weeks.cshtml", message);

        public Task<FileContentResult> ExcelNew(WinLossDetailExcelNewQuery message)
            => ExcelViewAsync(message);
    }
}