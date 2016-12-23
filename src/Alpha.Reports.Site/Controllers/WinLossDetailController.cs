namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.WinLossDetails;
    using ViewModels;

    public class WinLossDetailController : BaseReportController
    {
        private readonly IMediator mediator;

        public WinLossDetailController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ActionResult Index(WinLossDetailQuery message)
        {
            var viewModel = mediator.Send(message);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Excel(ExportWinLossDetailQuery message)
        {
            var excelFile = mediator.Send<ExcelFileViewModel>(message);

            return File(excelFile.Content, excelFile.ContentType, excelFile.Name);
        }
    }
}