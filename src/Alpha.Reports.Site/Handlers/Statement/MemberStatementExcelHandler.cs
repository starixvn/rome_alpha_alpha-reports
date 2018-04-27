namespace Alpha.Reports.Site.Handlers.Statement
{
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Params.Statement;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Requests.Statement;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;
    using ONEbook.SpreadSheets;

    public class MemberStatementExcelHandler
        : BaseHandler,
        IRequestHandler<MemberStatementExcelQuery, ExcelFileViewModel>
    {
        private readonly StatementService reportService;
        private readonly MemberStatementWorkbook workbook;

        public MemberStatementExcelHandler(StatementService reportService, MemberStatementWorkbook workbook)
        {
            this.reportService = reportService;
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(MemberStatementExcelQuery message)
        {
            var models = reportService.GetStatements(new GetStatementParam(message.CustId, message.FromDate, message.ToDate, message.LimitedDateToShowInfo));

            var fileContent = workbook
                .UseTemplate(GetTemplateInfo())
                .Title(GetReportTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(models, null)
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message), fileContent);
        }

        private string GetReportTitle(MemberStatementExcelQuery message)
             => string.Join(
                " ",
                Resources.Statement,
                $"({message.CustName})",
                message.FromDate.ToShortDateWithSlash(),
                "-->",
                message.ToDate.ToShortDateWithSlash());

        private TemplateInfo GetTemplateInfo()
            => new TemplateInfo(
                GetExcelTemplatePath("Member"),
                noInfomationText: Resources.NoInformationIsAvailable,
                titleStartIndex: 1,
                headerStartIndex: 2,
                bodyStartIndex: 3,
                bodyEndIndex: 4);

        private string GetExcelFileName(MemberStatementExcelQuery message)
            => string.Join(
                "-",
                Resources.Statement,
                message.CustName,
                message.FromDate.ToShortDateWithSlash(),
                message.ToDate.ToShortDateWithSlash());
    }
}