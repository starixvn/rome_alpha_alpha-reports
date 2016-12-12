namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using Excel.Services;
    using MediatR;
    using Requests.WinLoss;
    using Web;

    public class WinLossController : BaseController
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/WinLossDetail/";
        private readonly IMediator mediator;
        private readonly WinLossDetailExcelExporter excelExporter;

        public WinLossController(
            IMediator mediator,
            WinLossDetailExcelExporter excelExporter)
        {
            this.mediator = mediator;
            this.excelExporter = excelExporter;
        }

        public ActionResult WinLoss(WinLossQuery message)
        {
            var viewModel = mediator.Send(message);

            return View(viewModel);
        }

        public ActionResult EnteredResult(EnterResultQuery message)
        {
            var viewModel = mediator.Send(message);

            return View(viewModel);
        }
    }
}