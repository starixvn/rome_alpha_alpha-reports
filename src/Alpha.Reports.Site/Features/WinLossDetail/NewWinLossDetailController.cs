namespace Alpha.Reports.Site.Features.WinLossDetail
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Reports.Site.Controllers;
    using Alpha.Reports.Site.Features.WinLossDetail.Requests;
    using Alpha.Web.Filters;
    using MediatR;

    [AllowProtectedMode]
    public class NewWinLossDetailController : ReportBaseController
    {
        public NewWinLossDetailController(IMediator mediator) : base(mediator)
        {
        }

        [Route("newwinlossdetail/master")]
        [Route("newwinlossdetail/agent")]
        [Route("newwinlossdetail/member")]
        public Task<ActionResult> Index(NewWinLossDetailQuery message)
            => ViewAsync("~/Features/WinLossDetail/Views/Index.cshtml", message);

        public Task<ActionResult> GetData(NewWinLossDetailQuery message)
            => ViewAsync("~/Views/WinLossDetail/ReportData.cshtml", message);
    }
}