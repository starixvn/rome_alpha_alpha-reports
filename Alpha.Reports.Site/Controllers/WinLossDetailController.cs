namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using App_GlobalResources;
    using Excels;
    using Helpers;
    using MediatR;
    using Requests.WinLossDetails;

    public class WinLossDetailController : BaseController
    {
        private readonly IMediator mediator;
        private readonly WinLossDetailExcelExporter excelExporter;

        public WinLossDetailController(
            IMediator mediator,
            WinLossDetailExcelExporter excelExporter)
        {
            this.mediator = mediator;
            this.excelExporter = excelExporter;
        }

        public ActionResult Index(WinLossDetailQuery message)
        {
        [HttpGet]
        public ActionResult MemberExcel(WinLossDetailQuery<MemberWinLossDetail> message)
        {
            var excelTemplate = HttpContext.Server.MapPath(ExcelTemplatePath);

            var excelStream = excelExporter.Member(excelTemplate);

            return File(excelStream.ToArray(), ExcelHelper.ContentType, ExcelHelper.GetFileName(Resources.MemberWinLossDetail));
        }

            var viewModel = mediator.Send(message);

            return View(viewModel);
        }
    }
}