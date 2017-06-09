namespace Alpha.Reports.Site.Handlers.WinLossAnalysis
{
    using System;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Models.WinLossAnalysis;
    using Requests.WinLossAnalysis;
    using Services;
    using ViewModels;
    using ViewModels.WinLossAnalysis;
    using static ONEbook.SpreadSheets.Column;

    public class WinLossAnalysisExcelHandler :
        WinLossAnalysisBaseHandler,
        IRequestHandler<WinLossAnalysisExcelQuery, ExcelFileViewModel>
    {
        private readonly string reportTypeName = nameof(WinLossAnalysisReportTypeViewModel.ReportTypeName);
        private readonly Workbook<WinLossAnalysisModel> workbook;

        public WinLossAnalysisExcelHandler(
            ProductService productService,
            WinLossAnalysisService reportService,
            Workbook<WinLossAnalysisModel> workbook) : base(productService, reportService)
        {
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(WinLossAnalysisExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var winLossAnalysisParam = CreateWinLossAnalysisParamater(message);
            var reportData = reportService.GetWinLossAnalysis(winLossAnalysisParam);
            var grandTotal = AccumulateGrandTotal(reportData);

            var result = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Title(GetReportTitle(message))
                .Header(key => key.Equals(reportTypeName) ? message.ReportTypeName : Resources.ResourceManager.GetString(key))
                .Body(reportData, GetBodyRowMapper(message))
                .Footer(grandTotal, GetTotalRowMapper(message))
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, winLossAnalysisParam), result);
        }

        private Mapper<WinLossAnalysisModel> GetBodyRowMapper(WinLossAnalysisExcelQuery message)
          => GetRowMapper(message, isTotal: false);

        private Mapper<WinLossAnalysisModel> GetTotalRowMapper(WinLossAnalysisExcelQuery message)
            => GetRowMapper(message, isTotal: true);

        private Mapper<WinLossAnalysisModel> GetRowMapper(WinLossAnalysisExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<WinLossAnalysisModel>()
                .Map(A, winLossAnalysis => isTotal ? Resources.Total : winLossAnalysis.RollupKey)
                .Map(B, winLossAnalysis => winLossAnalysis.BetCount)
                .Map(C, winLossAnalysis => winLossAnalysis.Turnover)
                .Map(D, winLossAnalysis => winLossAnalysis.MemberNetWinLoss.WinLoss)
                .Map(E, winLossAnalysis => winLossAnalysis.MemberNetWinLoss.Comm)
                .Map(F, winLossAnalysis => winLossAnalysis.MemberNetWinLoss.Total)
                .Map(G, winLossAnalysis => winLossAnalysis.AgentNetWinLoss.WinLoss)
                .Map(H, winLossAnalysis => winLossAnalysis.AgentNetWinLoss.Comm)
                .Map(I, winLossAnalysis => winLossAnalysis.AgentNetWinLoss.Total);

            if (message.CustLevel.IsAgentLevel())
            {
                mapper.Map(J, winLossAnalysis => winLossAnalysis.Company);
            }

            if (message.CustLevel.IsMasterOrSuperLevel())
            {
                mapper
                    .Map(J, winLossAnalysis => winLossAnalysis.MasterNetWinLoss.WinLoss)
                    .Map(K, winLossAnalysis => winLossAnalysis.MasterNetWinLoss.Comm)
                    .Map(L, winLossAnalysis => winLossAnalysis.MasterNetWinLoss.Total);
            }

            if (message.CustLevel.IsMasterLevel())
            {
                mapper.Map(M, winLossAnalysis => winLossAnalysis.Company);
            }

            if (message.CustLevel.IsSuperLevel())
            {
                mapper
                    .Map(M, winLossAnalysis => winLossAnalysis.SuperNetWinLoss.WinLoss)
                    .Map(N, winLossAnalysis => winLossAnalysis.SuperNetWinLoss.Comm)
                    .Map(O, winLossAnalysis => winLossAnalysis.SuperNetWinLoss.Total)
                    .Map(P, winLossAnalysis => winLossAnalysis.Company);
            }

            return mapper;
        }

        private TemplateInfo GetTemplateInfo(WinLossAnalysisExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable);

        private string GetReportTitle(WinLossAnalysisExcelQuery message)
            => $"{message.ReportTypeTitle}" +
               $" {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}" +
               $" {Resources.of2} {productService.ConcatProductNames(message.UserSelectedProductIds)}";

        private string GetExcelFileName(WinLossAnalysisExcelQuery message, WinLossAnalysisParam param)
           => string.Join(
               "-",
               $"{message.ReportTypeTitle}",
               message.CustName,
               param.FromDate.ToShortDateWithSlash(),
               param.ToDate.ToShortDateWithSlash());
    }
}