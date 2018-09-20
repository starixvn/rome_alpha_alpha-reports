namespace Alpha.Reports.Site.Handlers.PokerGamesWinLossDetail
{
    using System;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Models.PokerGamesWinLossDetail;
    using Requests.PokerGamesWinLossDetail;
    using Services;
    using ViewModels;
    using static ONEbook.SpreadSheets.Column;

    public class PokerGamesWinLossDetailExcelHandler :
        PokerGamesWinLossDetailBaseHandler,
        IRequestHandler<PokerGamesWinLossDetailExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<PokerGamesWinLossDetailModel> workbook;

        public PokerGamesWinLossDetailExcelHandler(
            PokerGamesWinLossDetailService reportService,
            Workbook<PokerGamesWinLossDetailModel> workbook) : base(reportService)
        {
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(PokerGamesWinLossDetailExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var pokerGamesWinLossDetailParam = CreateParameter(message, message.CustLevel);

            var pokerGamesReportData = message.ReportData ?? reportService.GetWinLossDetail(pokerGamesWinLossDetailParam);

            var fileContent = workbook
               .UseTemplate(GetTemplateInfo(message))
               .Title(GetReportTitle(message))
               .Header(Resources.ResourceManager.GetString)
               .Body(pokerGamesReportData.DataSource, GetBodyRowMapper(message))
               .Footer(pokerGamesReportData.TotalData, GetTotalRowMapper(message))
               .Sheet(Resources.DefaultSheetName)
               .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, pokerGamesWinLossDetailParam), fileContent);
        }

        private string GetReportTitle(PokerGamesWinLossDetailExcelQuery message)
            => $"{Resources.PokerGames_WinLoss} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}";

        private string GetExcelFileName(PokerGamesWinLossDetailExcelQuery message, PokerGamesWinLossDetailParam winLossDetailParam)
            => string.Join(
                "-",
                Resources.PokerGames_WinLoss,
                message.CustName,
                winLossDetailParam.FromDate.ToShortDateWithSlash(),
                winLossDetailParam.ToDate.ToShortDateWithSlash());

        private TemplateInfo GetTemplateInfo(PokerGamesWinLossDetailExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable)
            .FreezePanel(4, B);

        private Mapper<PokerGamesWinLossDetailModel> GetBodyRowMapper(PokerGamesWinLossDetailExcelQuery message)
            => GetRowMapper(message, isTotal: false);

        private Mapper<PokerGamesWinLossDetailModel> GetTotalRowMapper(PokerGamesWinLossDetailExcelQuery message)
            => GetRowMapper(message, isTotal: true);

        internal Mapper<PokerGamesWinLossDetailModel> GetRowMapper(PokerGamesWinLossDetailExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<PokerGamesWinLossDetailModel>()
                .Map(A, winLossDetail => isTotal ? Resources.Total : winLossDetail.CustName)
                .Map(B, winLossDetail => winLossDetail.RoundCount)
                .Map(C, winLossDetail => winLossDetail.Turnover)
                .Map(D, winLossDetail => winLossDetail.NetTurnover)
                .Map(E, winLossDetail => winLossDetail.TaxComm)
                .Map(F, winLossDetail => winLossDetail.MemberWinLoss.WinLoss)
                .Map(G, winLossDetail => winLossDetail.MemberWinLoss.InsuranceGemsWinLoss)
                .Map(H, winLossDetail => winLossDetail.MemberWinLoss.Comm)
                .Map(I, winLossDetail => winLossDetail.MemberWinLoss.Total)
                .Map(J, winLossDetail => winLossDetail.AgentWinLoss.InsuranceGemsWinLoss)
                .Map(K, winLossDetail => winLossDetail.AgentWinLoss.Comm)
                .Map(L, winLossDetail => winLossDetail.AgentWinLoss.Total);

            if (message.CustLevel.IsAgentLevel())
            {
                mapper.Map(M, winLossDetail => winLossDetail.Company);
            }

            if (message.CustLevel.IsMasterOrSuperLevel())
            {
                mapper
                    .Map(M, winLossDetail => winLossDetail.MasterWinLoss.InsuranceGemsWinLoss)
                    .Map(N, winLossDetail => winLossDetail.MasterWinLoss.Comm)
                    .Map(O, winLossDetail => winLossDetail.MasterWinLoss.Total);
            }

            if (message.CustLevel.IsMasterLevel())
            {
                mapper.Map(P, winLossDetail => winLossDetail.Company);
            }

            if (message.CustLevel.IsSuperLevel())
            {
                mapper
                    .Map(P, winLossDetail => winLossDetail.SuperWinLoss.InsuranceGemsWinLoss)
                    .Map(Q, winLossDetail => winLossDetail.SuperWinLoss.Comm)
                    .Map(R, winLossDetail => winLossDetail.SuperWinLoss.Total)
                    .Map(S, winLossDetail => winLossDetail.Company);
            }

            return mapper;
        }
    }
}