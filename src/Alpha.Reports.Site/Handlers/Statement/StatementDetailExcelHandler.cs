namespace Alpha.Reports.Site.Handlers.Statement
{
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models.Statement;
    using Alpha.Reports.Params.Statement;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Requests.Statement;
    using Alpha.Reports.Site.ViewModels;
    using Alpha.Reports.Site.ViewModels.Statement;
    using MediatR;
    using ONEbook.SpreadSheets;
    using static ONEbook.SpreadSheets.Column;

    public class StatementDetailExcelHandler
        : BaseHandler,
        IRequestHandler<StatementDetailExcelQuery, ExcelFileViewModel>
    {
        private readonly StatementService reportService;
        private readonly Workbook<StatementDetailModel> workbook;

        public StatementDetailExcelHandler(StatementService reportService, Workbook<StatementDetailModel> workbook)
        {
            this.reportService = reportService;
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(StatementDetailExcelQuery message)
        {
            var models = reportService.StatementDetails(new StatementDetailsParam(message.CustId, message.TransferDate));

            var templateInfo = new TemplateInfo(
                GetExcelTemplatePath("StatementDetails"),
                noInfomationText: Resources.NoInformationIsAvailable,
                titleStartIndex: 1,
                headerStartIndex: 2,
                bodyStartIndex: 3,
                bodyEndIndex: 4);

            var reportTitle = string.Join(" ", Resources.TransferDetails, $"({message.CustName}):", message.TransferDate.ToShortDateWithSlash());

            var fileContent = workbook
                  .UseTemplate(templateInfo)
                  .Title(reportTitle)
                  .Header(Resources.ResourceManager.GetString)
                  .Body(models, GetBodyRowMapper())
                  .Footer(StatementDetailViewModel.AccummulateTotal(models), GetFooterRowMapper())
                  .Sheet(Resources.DefaultSheetName)
                  .ExportToBytes();

            var excelFileName = string.Join("-", Resources.TransferDetails, message.CustName, message.TransferDate.ToShortDateWithSlash());

            return new ExcelFileViewModel(excelFileName, fileContent);
        }

        internal Mapper<StatementDetailModel> GetBodyRowMapper()
        {
            var mapper = new Mapper<StatementDetailModel>()
                .Map(A, statement => string.Join("\r\n", statement.FullTransDateName, statement.TransDateString, statement.TransTimeString))
                .Map(B, statement => string.Join("\r\n", statement.TransType, statement.TransDesc))
                .Map(C, statement => (object)statement.Amount ?? Resources.Symbol_NoDataNumber);

            return mapper;
        }

        internal Mapper<StatementDetailModel> GetFooterRowMapper()
        {
            var mapper = new Mapper<StatementDetailModel>()
                .Map(A, statement => Resources.Total)
                .Map(C, statement => (object)statement.Amount ?? Resources.Symbol_NoDataNumber);

            return mapper;
        }
    }
}