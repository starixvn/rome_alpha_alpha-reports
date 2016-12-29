namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System;
    using System.Linq;
    using App_GlobalResources;
    using Extensions;
    using Fanex.Common;
    using Helpers;
    using MediatR;
    using Models.WinLossDetail;
    using ONEbook.SpreadSheets;
    using Requests.WinLossDetail;
    using Services;
    using ViewModels;
    using static ONEbook.SpreadSheets.Column;

    public class WinLossDetailExcelHandler :
        BaseWinLossDetailHandler,
        IRequestHandler<WinLossDetailExcelQuery, ExcelFileViewModel>
    {
        private readonly ProductService productService;
        private readonly WinLossDetailService reportService;
        private readonly Workbook<WinLossDetailModel> workbook;

        public WinLossDetailExcelHandler(
            ProductService productService,
            WinLossDetailService reportService,
            Workbook<WinLossDetailModel> workbook) : base(productService, reportService)
        {
            this.productService = productService;
            this.reportService = reportService;
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(WinLossDetailExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var title = GetReportTitle(message.UserLevel);
            var products = string.IsNullOrWhiteSpace(message.UserSelectedProductIds)
                ? Enumerable.Empty<int>()
              : message.UserSelectedProductIds.Split(',').Select(s => Converter.ToInt(s));
            var reportTitle = $"{title} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}" +
                $" {Resources.of2} " + string.Join(", ", productService.GetProductNames(products));

            var winLossDetailParam = CreateParameter(message, message.UserLevel);
            var reportData = reportService.GetWinLossDetail(winLossDetailParam);

            var templateInfo = new TemplateInfo(templatePath: message.TemplatePath, noInfomationText: Resources.NoInformationIsAvailable)
                 .FreezePanel(4, B);
            var fileContent = workbook
                .UseTemplate(templateInfo)
                .Title(reportTitle)
                .Header(ExcelHelper.HeaderTitleMapping)
                .Body(reportData.DataSource, GetRowMapper(message, isTotal: false))
                .Footer(reportData.TotalData, GetRowMapper(message, isTotal: true))
                .Sheet(title)
                .ExportToBytes();

            return new ExcelFileViewModel(
                $"{title}-{winLossDetailParam.FromDate.ToShortDateWithSlash()}-{winLossDetailParam.ToDate.ToShortDateWithSlash()}",
                fileContent);
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