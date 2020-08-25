namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Models.WinLossDetail;
    using Requests.Consolidation;
    using Requests.WinLossDetail;
    using Services;
    using ViewModels;
    using static ONEbook.SpreadSheets.Column;

    public class WinLossDetailExcelHandler :
        WinLossDetailBaseHandler,
        IRequestHandler<WinLossDetailExcelQuery, ExcelFileViewModel>,
        IRequestHandler<ConsolidateWinLossDetailExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<WinLossDetailModel> workbook;

        public WinLossDetailExcelHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossDetailService reportService,
            Workbook<WinLossDetailModel> workbook) : base(productService, productDetailService, reportService)
        {
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(WinLossDetailExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var winLossDetailParam = CreateParameter(message, message.CustLevel);

            var reportData = message.ReportData ?? reportService.GetWinLossDetail(winLossDetailParam);

            var fileContent = workbook
               .UseTemplate(GetTemplateInfo(message))
               .Title(GetReportTitle(message))
               .Header(Resources.ResourceManager.GetString)
               .Body(reportData.DataSource, GetBodyRowMapper(message))
               .Footer(reportData.TotalData, GetTotalRowMapper(message))
               .Sheet(Resources.DefaultSheetName)
               .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, winLossDetailParam), fileContent);
        }

        public ExcelFileViewModel Handle(ConsolidateWinLossDetailExcelQuery message)
            => Handle((WinLossDetailExcelQuery)message);

        private string GetReportTitle(WinLossDetailExcelQuery message)
            => $"{Resources.WinLossDetail} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}" +
            $" {Resources.of2} {productService.ConcatProductNames(message.UserSelectedProductIds)}";

        private string GetExcelFileName(WinLossDetailExcelQuery message, WinLossDetailParam winLossDetailParam)
            => string.Join(
                "-",
                Resources.WinLossDetail,
                message.CustName,
                winLossDetailParam.FromDate.ToShortDateWithSlash(),
                winLossDetailParam.ToDate.ToShortDateWithSlash());

        private TemplateInfo GetTemplateInfo(WinLossDetailExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable)
            .FreezePanel(4, B);

        private Mapper<WinLossDetailModel> GetBodyRowMapper(WinLossDetailExcelQuery message)
            => GetRowMapper(message, isTotal: false);

        private Mapper<WinLossDetailModel> GetTotalRowMapper(WinLossDetailExcelQuery message)
            => GetRowMapper(message, isTotal: true);

        internal Mapper<WinLossDetailModel> GetRowMapper(WinLossDetailExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<WinLossDetailModel>()
                .Map(A, winLossDetail => isTotal ? Resources.Total : winLossDetail.CustName)
                .Map(B, winLossDetail => winLossDetail.BetCount)
                .Map(C, winLossDetail => winLossDetail.Turnover)
                .Map(D, winLossDetail => winLossDetail.NetTurnover)
                .Map(E, winLossDetail => winLossDetail.GrossComm)
                .Map(F, winLossDetail => winLossDetail.MemberNetWinLoss.WinLoss)
                .Map(G, winLossDetail => winLossDetail.MemberNetWinLoss.Comm)
                .Map(H, winLossDetail => winLossDetail.MemberNetWinLoss.Total)
                .Map(I, winLossDetail => winLossDetail.AgentNetWinLoss.WinLoss)
                .Map(J, winLossDetail => winLossDetail.AgentNetWinLoss.Comm)
                .Map(K, winLossDetail => winLossDetail.AgentNetWinLoss.Total);

            if (message.CustLevel.IsAgentLevel())
            {
                mapper.Map(L, winLossDetail => winLossDetail.Company);
            }

            if (message.CustLevel.IsMasterOrSuperLevel())
            {
                mapper
                    .Map(L, winLossDetail => winLossDetail.MasterNetWinLoss.WinLoss)
                    .Map(M, winLossDetail => winLossDetail.MasterNetWinLoss.Comm)
                    .Map(N, winLossDetail => winLossDetail.MasterNetWinLoss.Total);
            }

            if (message.CustLevel.IsMasterLevel())
            {
                mapper.Map(O, winLossDetail => winLossDetail.Company);
            }

            if (message.CustLevel.IsSuperLevel())
            {
                mapper
                    .Map(O, winLossDetail => winLossDetail.SuperNetWinLoss.WinLoss)
                    .Map(P, winLossDetail => winLossDetail.SuperNetWinLoss.Comm)
                    .Map(Q, winLossDetail => winLossDetail.SuperNetWinLoss.Total)
                    .Map(R, winLossDetail => winLossDetail.Company);
            }

            return mapper;
        }
    }
}