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

        #region Old View
        [Route("winlossdetail/masterold")]
        [Route("winlossdetail/agentold")]
        [Route("winlossdetail/memberold")]
        public ActionResult Index(WinLossDetailQuery message)
            => View<WinLossDetailViewModel>($"{BasePath}/{nameof(Index)}.cshtml", message);

        [HttpGet]
        public ActionResult Excel(WinLossDetailExcelQuery message)
            => ExcelView(message);
        #endregion

        public ActionResult ReportLegend(ReportLegendQuery message)
            => View($"{BasePath}/{nameof(ReportLegend)}.cshtml", message);

        #region New View
        [Route("winlossdetail/masternew")]
        [Route("winlossdetail/agentnew")]
        [Route("winlossdetail/membernew")]
        public Task<ActionResult> IndexNew(WinLossDetailNewQuery message)
            => ViewAsync<WinLossDetailNewViewModel>($"{BasePath}/{nameof(IndexNew)}.cshtml", message);

        public Task<ActionResult> GetData(WinLossDetailGetDataQuery message)
            => ViewAsync<WinLossDetailGetDataViewModel>($"{BasePath}/ReportDataNew.cshtml", message);

        public Task<ActionResult> GetDataWeek(WinLossDetailGetDataWeekQuery message)
            => ViewAsync($"{BasePath}/ReportDataWeek.cshtml", message);

        public Task<FileContentResult> ExcelNew(WinLossDetailExcelNewQuery message)
            => ExcelViewAsync(message);

        public Task<ActionResult> GetWeeks(WinLossDetailWeeksQuery message)
            => ViewAsync<IEnumerable<WeekSelectionViewModel>>($"{BasePath}/Weeks.cshtml", message);
        #endregion

        #region New View with REWARD column
        public Task<ActionResult> Index2(WinLossDetailNewQuery message)
        {
            message.IsReward = true;

            return ViewAsync<WinLossDetailNewViewModel>($"{BasePath}/{nameof(Index2)}.cshtml", message);
        }

        public Task<ActionResult> GetData2(WinLossDetailGetDataQuery message)
        {
            message.IsReward = true;

            return ViewAsync<WinLossDetailGetDataViewModel>($"{BasePath}/ReportData2.cshtml", message);
        }

        public Task<ActionResult> GetDataWeek2(WinLossDetailGetDataWeekQuery message)
        {
            message.IsReward = true;

            return ViewAsync($"{BasePath}/ReportDataWeek2.cshtml", message);
        }

        public Task<FileContentResult> Excel2(WinLossDetailExcel2Query message)
        {
            message.IsReward = true;

            return ExcelViewAsync(message);
        }

        public Task<ActionResult> GetWeeks2(WinLossDetailWeeksQuery message)
        {
            message.IsReward = true;

            return ViewAsync<IEnumerable<WeekSelectionViewModel>>($"{BasePath}/Weeks.cshtml", message);
        }
        #endregion
    }
}