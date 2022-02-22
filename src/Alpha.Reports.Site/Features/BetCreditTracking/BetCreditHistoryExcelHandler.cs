namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Params;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;
    using ONEbook.SpreadSheets;
    using System;
    using System.Threading.Tasks;
    using static ONEbook.SpreadSheets.Column;

    public class BetCreditHistoryExcelHandler : BetCreditHistoryBaseHandler, IAsyncRequestHandler<BetCreditHistoryExcelQuery, ExcelFileViewModel>
    {
        private const int TitleStartIndex = 1;
        private const int HeaderStartIndex = 2;
        private const int BodyStartIndex = 3;
        private const int BodyEndIndex = 4;
        private const int FreezeRowIndex = 3;
        private const int SheetIndex = 1;
        private const int FooterEndIndex = 5;

        private readonly BetCreditHistoryWorkbook workbook;

        public BetCreditHistoryExcelHandler(BetCreditHistoryService reportService, BetCreditHistoryWorkbook workbook) : base(reportService)
        {
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(BetCreditHistoryExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var betCreditHistoryParam = CreateParameter(message);

            var reportData = message.ReportData ?? await reportService.GetBetCreditHistoryAsync(betCreditHistoryParam);

            var fileContent = workbook
                .SetData(reportData.DataSource)
                .SetRowMappers(GetRowMapper())
                .UseTemplate(GetTemplateInfo())
                .Title(GetReportTitle(message,betCreditHistoryParam))
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData.DataSource, GetRowMapper())
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(betCreditHistoryParam), fileContent);
        }

        private TemplateInfo GetTemplateInfo()
            => new TemplateInfo(
                    GetExcelTemplatePath("BetCreditHistory"),
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

        private string GetReportTitle(BetCreditHistoryExcelQuery message, BetCreditHistoryParam param)
        {
            return $"{Resources.BetCreditHistory} {param.FromDate.ToShortDateWithSlash()} -->  {param.ToDate.ToShortDateWithSlash()} - Member: {message.CustName}";
        }

        private string GetExcelFileName(BetCreditHistoryParam param)
            => string.Join(
                "-",
                Resources.BetCreditHistory,
                param.FromDate.ToShortDateWithSlash(),
                param.ToDate.ToShortDateWithSlash());

        internal Mapper<BetCreditHistoryModel> GetRowMapper()
        {
            return new Mapper<BetCreditHistoryModel>()
                .Map(A, betCreditHistory => betCreditHistory.ActionDateTime.ToLongDate24HourWithSlash())
                .Map(B, betCreditHistory => betCreditHistory.WinLossDate.ToShortDateWithSlash())
                .Map(C, betCreditHistory => betCreditHistory.RefNo)
                .Map(D, betCreditHistory => betCreditHistory.ActualStake)
                .Map(E, betCreditHistory => betCreditHistory.WinLoss)
                .Map(F, betCreditHistory => betCreditHistory.PlayerComm)
                .Map(G, betCreditHistory => betCreditHistory.BalanceChange)
                .Map(H, betCreditHistory => betCreditHistory.NewBalance)
                .Map(I, betCreditHistory => betCreditHistory.GivenCredit)
                .Map(J, betCreditHistory => betCreditHistory.BetCredit)
                .Map(K, betCreditHistory => betCreditHistory.Action);
        }
    }
}