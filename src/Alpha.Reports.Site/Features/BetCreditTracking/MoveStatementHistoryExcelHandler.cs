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
    using static ONEbook.SpreadSheets.Column;

    public class MoveStatementHistoryExcelHandler : MoveStatementHistoryBaseHandler, IRequestHandler<MoveStatementHistoryExcelQuery, ExcelFileViewModel>
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

        public ExcelFileViewModel Handle(MoveStatementHistoryExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var moveStatementHistoryParam = CreateParameter(message);

            var result = reportService.GetMoveStatementHistory(moveStatementHistoryParam);

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

        private Mapper<MoveStatementHistoryByRefNoModel> GetMemberMapper()
            => GetRowMapper(true);

        private Mapper<MoveStatementHistoryByRefNoModel> GetRefNoMapper()
            => GetRowMapper(false);

        internal Mapper<MoveStatementHistoryByRefNoModel> GetRowMapper(bool isMember)
        {
            if (isMember)
            {
                return new Mapper<MoveStatementHistoryByRefNoModel>()
                    .Map(A, moveStatementHistory => moveStatementHistory.Username);
            }

            return new Mapper<MoveStatementHistoryByRefNoModel>()
                .Map(A, moveStatementHistory => moveStatementHistory.TransId)
                .Map(B, moveStatementHistory => moveStatementHistory.OldEventDate.ToShortDateWithSlash())
                .Map(C, moveStatementHistory => moveStatementHistory.NewEventDate.ToShortDateWithSlash())
                .Map(D, moveStatementHistory => moveStatementHistory.MemberWinLoss)
                .Map(E, moveStatementHistory => moveStatementHistory.MemberComm)
                .Map(F, moveStatementHistory => moveStatementHistory.MemberTotal);
        }
    }
}