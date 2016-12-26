namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.WinLoss;
    using Requests.WinLossDetails;
    using ViewModels;

    public class WinLossController : BaseReportController
    {
        private readonly IMediator mediator;

        public WinLossController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ActionResult EnteredResult(EnteredResultQuery message)
        {
            var viewModel = mediator.Send(message);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult EnteredResultExcel(ExportEnteredResultQuery message)
        {
            var excelFile = mediator.Send<ExcelFileViewModel>(message);

            return File(excelFile.Content, excelFile.ContentType, excelFile.Name);
        }

        public ActionResult WinLoss(WinLossQuery message)
        {
            var viewModel = mediator.Send(message);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult WinLossExcel(ExportWinLossQuery message)
        {
            var excelFile = mediator.Send<ExcelFileViewModel>(message);

            return File(excelFile.Content, excelFile.ContentType, excelFile.Name);
        }
    }
}