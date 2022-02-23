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
        private const int TitleStartIndex = 1;
        private const int HeaderStartIndex = 2;
        private const int BodyStartIndex = 3;
        private const int BodyEndIndex = 4;
        private const int FreezeRowIndex = 3;
        private const int SheetIndex = 1;
        private const int FooterEndIndex = 5;

        private readonly StatementMovementWorkbook workbook;

        public StatementMovementExcelHandler(StatementMovementService reportService, StatementMovementWorkbook workbook) : base(reportService)
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

            var reportData = message.ReportData ?? await reportService.GetStatementMovementAsync(statementMovementParam);

            var fileContent = workbook
                .SetData(reportData.DataSource, reportData.TotalData)
                .SetRowMappers(GetBodyRowMapper(), GetTotalRowMapper())
                .UseTemplate(GetTemplateInfo())
                .Title(GetReportTitle(statementMovementParam))
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData.DataSource, GetBodyRowMapper())
                .Footer(reportData.TotalData, GetTotalRowMapper())
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, statementMovementParam), fileContent);
        }

        private TemplateInfo GetTemplateInfo()
            => new TemplateInfo(
                    GetExcelTemplatePath("StatementMovement"), 
                    noInfomationText: Resources.NoInformationIsAvailable,
                    titleStartIndex: TitleStartIndex,
                    headerStartIndex: HeaderStartIndex,
                    bodyStartIndex: BodyStartIndex,
                    bodyEndIndex: BodyEndIndex,
                    freezeRowIndex: FreezeRowIndex,
                    freezeColumnName: A,
                    columnStartName: A,
                    sheetIndex: SheetIndex,
                    footerEndIndex: FooterEndIndex
                );

        private string GetReportTitle(StatementMovementParam param)
        {
            return $"{Resources.MembersMovedEventDateReport} {param.FromDate.ToShortDateWithSlash()} -->  {param.ToDate.ToShortDateWithSlash()}";
        }

        private string GetExcelFileName(StatementMovementExcelQuery message, StatementMovementParam param)
            => string.Join(
                "-",
                Resources.StatementMovement,
                message.CustName,
                param.FromDate.ToShortDateWithSlash(),
                param.ToDate.ToShortDateWithSlash());

        private Mapper<StatementMovementModel> GetBodyRowMapper()
           => GetRowMapper(isTotal: false);

        private Mapper<StatementMovementModel> GetTotalRowMapper()
            => GetRowMapper(isTotal: true);

        internal Mapper<StatementMovementModel> GetRowMapper(bool isTotal)
        {
            return new Mapper<StatementMovementModel>()
                .Map(A, statementMovement => isTotal ? Resources.Total : statementMovement.MemberRefNo)
                .Map(B, statementMovement => isTotal ? string.Empty : statementMovement.OldEventDate.ToShortDateWithSlash())
                .Map(C, statementMovement => isTotal ? string.Empty : statementMovement.NewEventDate.ToShortDateWithSlash())
                .Map(D, statementMovement => statementMovement.MemberWinLoss)
                .Map(E, statementMovement => statementMovement.MemberComm)
                .Map(F, statementMovement => statementMovement.MemberTotal);
        }
    }
}