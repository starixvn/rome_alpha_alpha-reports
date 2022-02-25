namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;
    using ONEbook.SpreadSheets;
    using System;
    using System.Threading.Tasks;
    using static ONEbook.SpreadSheets.Column;

    public class MoveStatementHistoryExcelHandler : MoveStatementHistoryBaseHandler, IAsyncRequestHandler<MoveStatementHistoryExcelQuery, ExcelFileViewModel>
    {
        private const int TitleStartIndex = 1;
        private const int HeaderStartIndex = 2;
        private const int BodyStartIndex = 3;
        private const int BodyEndIndex = 4;
        private const int FreezeRowIndex = 3;
        private const int SheetIndex = 1;
        private const int FooterEndIndex = 5;

        private readonly MoveStatementHistoryWorkbook workbook;

        public MoveStatementHistoryExcelHandler(MoveStatementHistoryService reportService, MoveStatementHistoryWorkbook workbook) : base(reportService)
        {
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(MoveStatementHistoryExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var moveStatementHistoryParam = CreateParameter(message);

            var reportData = message.ReportData ?? await reportService.GetMoveStatementHistoryAsync(moveStatementHistoryParam);

            var fileContent = workbook
                .SetData(reportData.DataSource, reportData.TotalData)
                .SetRowMappers(GetBodyRowMapper(), GetTotalRowMapper())
                .UseTemplate(GetTemplateInfo())
                .Title(GetReportTitle(moveStatementHistoryParam))
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData.DataSource, GetBodyRowMapper())
                .Footer(reportData.TotalData, GetTotalRowMapper())
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, moveStatementHistoryParam), fileContent);
        }

        private TemplateInfo GetTemplateInfo()
            => new TemplateInfo(
                    GetExcelTemplatePath("MoveStatementHistory"), 
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

        private string GetReportTitle(MoveStatementHistoryParam param)
        {
            return $"{Resources.MoveStatementHistory} {param.FromDate.ToShortDateWithSlash()} -->  {param.ToDate.ToShortDateWithSlash()}";
        }

        private string GetExcelFileName(MoveStatementHistoryExcelQuery message, MoveStatementHistoryParam param)
            => string.Join(
                "-",
                Resources.MoveStatementHistory,
                message.CustName,
                param.FromDate.ToShortDateWithSlash(),
                param.ToDate.ToShortDateWithSlash());

        private Mapper<MoveStatementHistoryModel> GetBodyRowMapper()
           => GetRowMapper(isTotal: false);

        private Mapper<MoveStatementHistoryModel> GetTotalRowMapper()
            => GetRowMapper(isTotal: true);

        internal Mapper<MoveStatementHistoryModel> GetRowMapper(bool isTotal)
        {
            return new Mapper<MoveStatementHistoryModel>()
                .Map(A, moveStatementHistory => isTotal ? Resources.Total : moveStatementHistory.MemberRefNo)
                .Map(B, moveStatementHistory => isTotal ? string.Empty : moveStatementHistory.OldEventDate.ToShortDateWithSlash())
                .Map(C, moveStatementHistory => isTotal ? string.Empty : moveStatementHistory.NewEventDate.ToShortDateWithSlash())
                .Map(D, moveStatementHistory => moveStatementHistory.MemberWinLoss)
                .Map(E, moveStatementHistory => moveStatementHistory.MemberComm)
                .Map(F, moveStatementHistory => moveStatementHistory.MemberTotal);
        }
    }
}