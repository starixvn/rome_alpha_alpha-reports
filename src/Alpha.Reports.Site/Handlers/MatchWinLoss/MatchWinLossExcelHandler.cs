namespace Alpha.Reports.Site.Handlers.MatchWinLoss
{
    using System;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params;
    using RemotingRef;
    using Requests.MatchWinLoss;
    using Services;
    using ViewModels;
    using ViewModels.MatchWinLoss;
    using static ONEbook.SpreadSheets.Column;

    public class MatchWinLossExcelHandler :
        MatchWinLossBaseHandler,
        IRequestHandler<MatchWinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<MatchWinLossViewModel> workbook;

        public MatchWinLossExcelHandler(
            MatchWinLossService reportService,
            CacheRefService cacheRefService,
            Workbook<MatchWinLossViewModel> workbook) : base(reportService, cacheRefService)
        {
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(MatchWinLossExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var matchWinLossParam = new MatchWinLossParam(message.CustId, message.FromDate, message.ToDate);
            var rawData = reportService.GetMatchWinLoss(matchWinLossParam);
            var reportData = BuildViewModel(rawData);
            var fileContent = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Title(GetReportTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData, GetBodyRowMapper())
                .Footer(AccumulateGrandTotal(reportData), GetTotalRowMapper())
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(matchWinLossParam), fileContent);
        }

        private TemplateInfo GetTemplateInfo(MatchWinLossExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable,
                titleStartIndex: 1,
                headerStartIndex: 2,
                bodyStartIndex: 3,
                bodyEndIndex: 4,
                freezeRowIndex: 0,
                freezeColumnName: A,
                columnStartName: A,
                sheetIndex: 1,
                footerEndIndex: 5);

        private string GetReportTitle(MatchWinLossExcelQuery message)
            => $"{Resources.MatchWinLoss} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}";

        private string GetExcelFileName(MatchWinLossParam matchWinLossParam)
            => string.Join(
                "-",
                Resources.MatchWinLoss,
                matchWinLossParam.FromDate.ToShortDateWithSlash(),
                matchWinLossParam.ToDate.ToShortDateWithSlash());

        private Mapper<MatchWinLossViewModel> GetBodyRowMapper() => GetRowMapper(isTotal: false);

        private Mapper<MatchWinLossViewModel> GetTotalRowMapper() => GetRowMapper(isTotal: true);

        private Mapper<MatchWinLossViewModel> GetRowMapper(bool isTotal)
        {
            var mapper = new Mapper<MatchWinLossViewModel>()
                .Map(A, matchWinLoss => isTotal ? Resources.Total : matchWinLoss.EventInfo)
                .Map(B, matchWinLoss => matchWinLoss.BetTypeName)
                .Map(C, matchWinLoss => matchWinLoss.GrossComm)
                .Map(D, matchWinLoss => matchWinLoss.Commission)
                .Map(E, matchWinLoss => matchWinLoss.PositionTaking)
                .Map(F, matchWinLoss => matchWinLoss.Total);

            return mapper;
        }
    }
}