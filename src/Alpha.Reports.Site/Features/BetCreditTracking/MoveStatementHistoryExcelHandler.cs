namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using System.Threading.Tasks;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;
    using ONEbook.SpreadSheets;
    using static ONEbook.SpreadSheets.Column;

    public class MoveStatementHistoryExcelHandler :
        MoveStatementHistoryBaseHandler,
        IAsyncRequestHandler<MoveStatementHistoryExcelQuery, ExcelFileViewModel>
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
            var moveStatementHistoryParam = CreateExcelParameter(message);
            var result = await reportService.GetMoveStatementHistory(moveStatementHistoryParam);

            var data = message.Data ?? result.Data;

            var fileContent = workbook
                .SetData(data)
                .SetRowMappers(GetMemberMapper(), GetRefNoMapper())
                .UseTemplate(GetTemplateInfo())
                .Title(GetReportTitle(moveStatementHistoryParam))
                .Header(Resources.ResourceManager.GetString)
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

        private Mapper<MoveStatementHistoryByRefNo> GetMemberMapper()
            => GetRowMapper(true);

        private Mapper<MoveStatementHistoryByRefNo> GetRefNoMapper()
            => GetRowMapper(false);

        internal Mapper<MoveStatementHistoryByRefNo> GetRowMapper(bool isMember)
        {
            if (isMember)
            {
                return new Mapper<MoveStatementHistoryByRefNo>()
                    .Map(A, moveStatementHistory => moveStatementHistory.Username)
                    .Map(D, moveStatementHistory => moveStatementHistory.MemberWinLoss)
                    .Map(E, moveStatementHistory => moveStatementHistory.MemberComm)
                    .Map(F, moveStatementHistory => moveStatementHistory.MemberTotal);
            }

            return new Mapper<MoveStatementHistoryByRefNo>()
                .Map(A, moveStatementHistory => moveStatementHistory.TransId.ToString())
                .Map(B, moveStatementHistory => moveStatementHistory.OldEventDate.ToShortDateWithSlash())
                .Map(C, moveStatementHistory => moveStatementHistory.NewEventDate.ToShortDateWithSlash())
                .Map(D, moveStatementHistory => moveStatementHistory.MemberWinLoss)
                .Map(E, moveStatementHistory => moveStatementHistory.MemberComm)
                .Map(F, moveStatementHistory => moveStatementHistory.MemberTotal);
        }
    }
}