namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Features.BetCreditTracking.StatementMovement;
    using Alpha.Reports.Features.BetCreditTracking.StatementMovement.Models;
    using Alpha.Reports.Features.BetCreditTracking.StatementMovement.Params;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;
    using ONEbook.SpreadSheets;
    using System;
    using System.Threading.Tasks;
    using static ONEbook.SpreadSheets.Column;

    public class StatementMovementExcelHandler : StatementMovementBaseHandler, IAsyncRequestHandler<StatementMovementExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<StatementMovementModel> workbook;

        public StatementMovementExcelHandler(StatementMovementService reportService, Workbook<StatementMovementModel> workbook) : base(reportService)
        {
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(StatementMovementExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var statementMovementParam = CreateParameter(message);

            var reportData = message.ReportData ?? await reportService.GetStatementMovementDetailAsync(statementMovementParam);

            var fileContent = workbook
                .UseTemplate(GetTemplateInfo())
                .Title(GetReportTitle(statementMovementParam))
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData.DataSource, GetBodyRowMapper(message))
                .Footer(reportData.TotalData, GetTotalRowMapper(message))
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, statementMovementParam), fileContent);
        }

        private TemplateInfo GetTemplateInfo()
            => new TemplateInfo(
                    GetExcelTemplatePath("StatementMovement"), 
                    noInfomationText: Resources.NoInformationIsAvailable,
                    titleStartIndex: 1,
                    headerStartIndex: 2,
                    bodyStartIndex: 3,
                    bodyEndIndex: 4,
                    freezeRowIndex: 3,
                    freezeColumnName: A,
                    columnStartName: A,
                    sheetIndex: 1,
                    footerEndIndex: 5
                );

        private string GetReportTitle(StatementMovementParam param)
        {
            return $"{Resources.MembersMovedEventDateReport} {param.FromDate.ToShortDateWithSlash()} -->  {param.ToDate.ToShortDateWithSlash()}";
        }

        private string GetExcelFileName(StatementMovementExcelQuery message, StatementMovementParam param)
            => string.Join(
                "-",
                    
                message.CustName,
                param.FromDate.ToShortDateWithSlash(),
                param.ToDate.ToShortDateWithSlash());

        private Mapper<StatementMovementModel> GetBodyRowMapper(StatementMovementExcelQuery message)
           => GetRowMapper(message, isTotal: false);

        private Mapper<StatementMovementModel> GetTotalRowMapper(StatementMovementExcelQuery message)
            => GetRowMapper(message, isTotal: true);

        internal Mapper<StatementMovementModel> GetRowMapper(StatementMovementExcelQuery message, bool isTotal)
        {
            return new Mapper<StatementMovementModel>()
                .Map(A, statementMovement => isTotal ? Resources.Total : statementMovement.MemberRefNo)
                .Map(B, statementMovement => statementMovement.MemberWinLoss)
                .Map(C, statementMovement => statementMovement.MemberWinLoss)
                .Map(D, statementMovement => statementMovement.MemberComm)
                .Map(E, statementMovement => statementMovement.MemberTotal);
        }
    }
}