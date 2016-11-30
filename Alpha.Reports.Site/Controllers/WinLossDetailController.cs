namespace Alpha.Reports.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using Constants;
    using Excel.Helpers;
    using Extensions;
    using MediatR;
    using Requests.WinLossDetails;
    using Web;

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
        public ActionResult Excel(WinLossDetailQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            message.PageIndex = DefaultVariables.PageIndex;
            message.PageSize = DefaultVariables.MaxRowExcelFile;

            var viewModel = mediator.Send(message);
            var fileContent = mediator.Send(new ExportWinLossDetailQuery(viewModel));

            return File(
                fileContent,
                ExcelHelper.ContentType,
                ExcelHelper.GetFileName($"{viewModel.Title}-{viewModel.FromDate.ToShortDateWithSlash()}-{viewModel.ToDate.ToShortDateWithSlash()}"));
        }
    }
}