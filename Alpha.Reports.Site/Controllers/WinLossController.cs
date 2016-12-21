namespace Alpha.Reports.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Extensions;
    using Helpers;
    using MediatR;
    using Requests.WinLoss;
    using Requests.WinLossDetails;

    public class WinLossController : BaseReportController
    {
        private readonly IMediator mediator;

        public WinLossController(
            IMediator mediator)
        {
            this.mediator = mediator;
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
                throw new ArgumentNullException("message", "message cannot be a null reference");
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