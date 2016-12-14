namespace Alpha.Reports.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using AppResources;
    using Excel.Helpers;
    using Excel.Services;
    using Extensions;
    using MediatR;
    using Requests.WinLoss;
    using Requests.WinLossDetails;
    using Web;

    public class WinLossController : BaseController
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/WinLoss/";
        private readonly IMediator mediator;
        private readonly WinLossExcelExporter excelExporter;

        public WinLossController(
            IMediator mediator,
            WinLossExcelExporter excelExporter)
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

        [HttpGet]
        public ActionResult WinLossExcel(WinLossQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var viewModel = mediator.Send(message);
            var fileContent = mediator.Send(new ExportWinLossQuery(viewModel));

            return File(
                fileContent,
                ExcelHelper.ContentType,
                ExcelHelper.GetFileName($"{viewModel.Title}-{viewModel.FromDate.ToShortDateWithSlash()}-{viewModel.ToDate.ToShortDateWithSlash()}"));
        }

        [HttpGet]
        public ActionResult EnteredResultExcel(EnterResultQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var viewModel = mediator.Send(message);
            var fileContent = mediator.Send(new ExportEnteredResultQuery(viewModel));

            return File(
                fileContent,
                ExcelHelper.ContentType,
                ExcelHelper.GetFileName($"{Resources.MatchResultEntered}-{viewModel.FromDate.ToShortDateWithSlash()}-{viewModel.ToDate.ToShortDateWithSlash()}"));
        }
    }
}