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
            var viewModel = mediator.Send(message);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Excel(WinLossDetailQuery message)
        {
            var viewModel = mediator.Send(message);

            var excelTemplate = HttpContext.Server.MapPath(ExcelTemplatePath);

            var fileContents = excelExporter.Export(excelTemplate, viewModel);

            return File(fileContents, ExcelHelper.ContentType, ExcelHelper.GetFileName(Resources.MemberWinLossDetail));
        }
    }
}