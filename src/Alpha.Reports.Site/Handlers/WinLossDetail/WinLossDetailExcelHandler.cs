namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Models.WinLossDetail;
    using Requests.WinLossDetail;
    using Services;
    using ViewModels;
    using static ONEbook.SpreadSheets.Column;

    public class WinLossDetailExcelHandler :
        WinLossDetailBaseHandler,
        IRequestHandler<WinLossDetailExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<WinLossDetailModel> workbook;

        public WinLossDetailExcelHandler(
            ProductService productService,
            WinLossDetailService reportService,
            Workbook<WinLossDetailModel> workbook) : base(productService, reportService)
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
            var reportData = reportService.GetWinLossDetail(winLossDetailParam);
            var fileContent = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Title(GetReporTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData.DataSource, GetRowMapper(message, isTotal: false))
                .Footer(reportData.TotalData, GetRowMapper(message, isTotal: true))
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, winLossDetailParam), fileContent);
        }

        private string GetReporTitle(WinLossDetailExcelQuery message)
        {
            var products = message.UserSelectedProductIds.ToListInt();

            return $"{Resources.WinLossDetail} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}" +
                $" {Resources.of2} " + string.Join(", ", productService.GetProductNames(products));
        }

        private string GetExcelFileName(WinLossDetailExcelQuery message, WinLossDetailParam winLossDetailParam)
        {
            return $"{Resources.WinLossDetail}-{message.CustName}-{winLossDetailParam.FromDate.ToShortDateWithSlash()}-{winLossDetailParam.ToDate.ToShortDateWithSlash()}";
        }

        private TemplateInfo GetTemplateInfo(WinLossDetailExcelQuery message)
        {
            var templatePath = GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName);

            return new TemplateInfo(templatePath: templatePath, noInfomationText: Resources.NoInformationIsAvailable)
                 .FreezePanel(4, B);
        }

        private Mapper<WinLossDetailModel> GetRowMapper(WinLossDetailExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<WinLossDetailModel>()
                .Map(A, winLossDetail => isTotal ? Resources.Total : winLossDetail.CustName)
                .Map(B, winLossDetail => winLossDetail.NumberOfTickets)
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