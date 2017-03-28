using Alpha.Reports.Extensions;
using Alpha.Reports.Params.Statement;
using Alpha.Reports.Services;
using Alpha.Reports.Site.App_GlobalResources;
using Alpha.Reports.Site.Requests.Statement;
using Alpha.Reports.Site.ViewModels;
using MediatR;
using ONEbook.SpreadSheets;

namespace Alpha.Reports.Site.Handlers.Statement
{
    public class SMAStatementExcelHandler
        : BaseHandler,
        IRequestHandler<SMAStatementExcelQuery, ExcelFileViewModel>
    {
        private readonly StatementService reportService;
        private readonly SMAStatementWorkbook workbook;

        public SMAStatementExcelHandler(StatementService reportService, SMAStatementWorkbook workbook)
        {
            this.reportService = reportService;
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(SMAStatementExcelQuery message)
        {
            var models = reportService.GetStatements(new GetStatementParam(message.CustId, message.FromDate, message.ToDate));

            var fileContent = workbook
                .UseTemplate(GetTemplateInfo())
                .Title(GetReporTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(models, null)
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message), fileContent);
        }

        private string GetReporTitle(SMAStatementExcelQuery message)
             => string.Join(
                " ",
                Resources.Statement,
                $"({message.CustName})",
                message.FromDate.ToShortDateWithSlash(),
                "-->",
                message.ToDate.ToShortDateWithSlash());

        private TemplateInfo GetTemplateInfo()
            => new TemplateInfo(
                GetExcelTemplatePath("SMA"),
                noInfomationText: Resources.NoInformationIsAvailable,
                titleStartIndex: 1,
                headerStartIndex: 2,
                bodyStartIndex: 3,
                bodyEndIndex: 4);

        private string GetExcelFileName(SMAStatementExcelQuery message)
            => string.Join(
                "-",
                Resources.Statement,
                message.CustName,
                message.FromDate.ToShortDateWithSlash(),
                message.ToDate.ToShortDateWithSlash());
    }
}