namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Reports.Models;
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

            var reportNameByLevel = GetReportNameByLevel(message.UserLevel);
            var products = message.UserSelectedProductIds.ToListInt();
            var reportTitle = $"{reportNameByLevel} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}" +
                $" {Resources.of2} " + string.Join(", ", productService.GetProductNames(products));

            var winLossDetailParam = CreateParameter(message, message.UserLevel);
            var reportData = reportService.GetWinLossDetail(winLossDetailParam);
            var templatePath = GetExcelTemplatePath(GetTemplateName(message.UserLevel));

            var templateInfo = new TemplateInfo(templatePath: templatePath, noInfomationText: Resources.NoInformationIsAvailable)
                 .FreezePanel(4, B);
            var fileContent = workbook
                .UseTemplate(templateInfo)
                .Title(reportTitle)
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData.DataSource, GetRowMapper(message, isTotal: false))
                .Footer(reportData.TotalData, GetRowMapper(message, isTotal: true))
                .Sheet(reportNameByLevel)
                .ExportToBytes();

            return new ExcelFileViewModel(
                $"{reportNameByLevel}-{winLossDetailParam.FromDate.ToShortDateWithSlash()}-{winLossDetailParam.ToDate.ToShortDateWithSlash()}",
                fileContent);
        }

        internal string GetTemplateName(UserLevel userLevel) =>
                    userLevel.IsSuperLevel()
                        ? "Master"
                        : userLevel.IsMasterLevel()
                            ? "Agent"
                            : "Member";

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

            if (message.UserLevel.IsAgentLevel())
            {
                mapper.Map(L, winLossDetail => winLossDetail.Company);
            }

            if (message.UserLevel.IsMasterOrSuperLevel())
            {
                mapper
                    .Map(L, winLossDetail => winLossDetail.MasterNetWinLoss.WinLoss)
                    .Map(M, winLossDetail => winLossDetail.MasterNetWinLoss.Comm)
                    .Map(N, winLossDetail => winLossDetail.MasterNetWinLoss.Total);
            }

            if (message.UserLevel.IsMasterLevel())
            {
                mapper.Map(O, winLossDetail => winLossDetail.Company);
            }

            if (message.UserLevel.IsSuperLevel())
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