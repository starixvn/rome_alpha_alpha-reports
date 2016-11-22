namespace Alpha.Reports.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using Constants;
    using Excels.Helpers;
    using Excels.Params;
    using Excels.Services;
    using MediatR;
    using Models.WinlossDetails;
    using Requests.WinLossDetails;

    public class WinLossDetailController : BaseController
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/WinLossDetail/";
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
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            message.PageIndex = DefaultVariables.PageIndex;
            message.PageSize = DefaultVariables.MaxRowExcelFile;

            var viewModel = mediator.Send(message);

            var excelTemplatePath = ExcelTemplateDir +
                (viewModel.UserLevel.IsSuperLevel()
                ? "Master.xls"
                : viewModel.UserLevel.IsMasterLevel()
                ? "Agent.xls"
                : "Member.xls");

            var fileContents = excelExporter.Export(
                new ExportExcelParam<WinLossDetail>
                {
                    TemplatePath = HttpContext.Server.MapPath(excelTemplatePath),
                    Title = viewModel.Title,
                    SheetName = viewModel.Title,
                    ReportData = viewModel.ReportData,
                    CustId = viewModel.CustId,
                    FromDate = viewModel.FromDate,
                    ToDate = viewModel.ToDate
                },
                viewModel.UserLevel);

            return File(fileContents, ExcelHelper.ContentType, ExcelHelper.GetFileName(viewModel.Title));
        }
    }
}