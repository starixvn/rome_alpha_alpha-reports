////namespace Alpha.Reports.Site.Handlers.WinLoss
////{
////    using System;
////    using App_GlobalResources;
////    using Extensions;
////    using MediatR;
////    using ONEbook.SpreadSheets;
////    using Params;
////    using Reports.Models.WinLossAnalysis;
////    using Requests.WinLossAnalysis;
////    using Services;
////    using ViewModels;

////    public class WinLossAnalysisExcelHandler :
////        WinLossAnalysisBaseHandler,
////        IRequestHandler<WinLossAnalysisExcelQuery, ExcelFileViewModel>
////    {
////        private readonly Workbook<WinLossAnalysisModel> workbook;

////        public WinLossAnalysisExcelHandler(
////            ProductService productService,
////            WinLossAnalysisService reportService,
////            Workbook<WinLossAnalysisModel> workbook) : base(productService, reportService)
////        {
////            this.workbook = workbook;
////        }

////        public ExcelFileViewModel Handle(WinLossAnalysisExcelQuery message)
////        {
////            if (message == null)
////            {
////                throw new ArgumentNullException(nameof(message));
////            }

////            var winLossAnalysisParam = CreateWinLossAnalysisParamater(message);
////            var reportData = reportService.GetWinLossinLossAnalysis(winLossAnalysisParam);
////            var grandTotal = AccumulateGrandTotal(reportData);

////            var result = workbook
////                .UseTemplate(GetTemplateInfo(message))
////                .Title(GetReporTitle(message))
////                .Header(Resources.ResourceManager.GetString)
////                .Body(reportData, GetBodyRowMapper(message))
////                .Footer(grandTotal, GetTotalRowMapper(message))
////                .Sheet(Resources.DefaultSheetName)
////                .ExportToBytes();

////            return new ExcelFileViewModel(GetExcelFileName(message, winLossAnalysisParam), result);
////        }

////        private Mapper<WinLossAnalysisModel> GetBodyRowMapper(WinLossAnalysisExcelQuery message)
////          => GetRowMapper(message, isTotal: false);

////        private Mapper<WinLossAnalysisModel> GetTotalRowMapper(WinLossAnalysisExcelQuery message)
////            => GetRowMapper(message, isTotal: true);

////        private Mapper<WinLossAnalysisModel> GetRowMapper(WinLossAnalysisExcelQuery message, bool isTotal)
////        {
////            var mapper = new Mapper<WinLossAnalysisModel>()
////                .Map(Column.A, winLoss => isTotal ? Resources.Total : winLoss.ReportTypeValue)
////                .Map(Column.B, winLoss => winLoss.Turnover)
////                .Map(Column.C, winLoss => winLoss.BetCount);

////            if (message.CustLevel.IsAgentLevel())
////            {
////                mapper.Map(Column.D, winLoss => winLoss.MemberNetWinLoss.WinLoss)
////                 .Map(Column.E, winLoss => winLoss.MemberNetWinLoss.Comm)
////                 .Map(Column.F, winLoss => winLoss.MemberNetWinLoss.Total)
////                 .Map(Column.G, winLoss => winLoss.AgentNetWinLoss.WinLoss)
////                 .Map(Column.H, winLoss => winLoss.AgentNetWinLoss.Comm)
////                 .Map(Column.I, winLoss => winLoss.AgentNetWinLoss.Total)
////                 .Map(Column.J, winLoss => winLoss.Company);
////            }

////            if (message.CustLevel.IsMasterLevel())
////            {
////                mapper.Map(Column.D, winLoss => winLoss.AgentNetWinLoss.Total)
////                   .Map(Column.E, winLoss => winLoss.MasterNetWinLoss.WinLoss)
////                   .Map(Column.F, winLoss => winLoss.MasterNetWinLoss.Comm)
////                   .Map(Column.G, winLoss => winLoss.MasterNetWinLoss.Total)
////                   .Map(Column.H, winLoss => winLoss.Company);
////            }

////            if (message.CustLevel.IsSuperLevel())
////            {
////                mapper.Map(Column.D, winLoss => winLoss.MasterNetWinLoss.Total)
////                    .Map(Column.E, winLoss => winLoss.SuperNetWinLoss.WinLoss)
////                    .Map(Column.F, winLoss => winLoss.SuperNetWinLoss.Comm)
////                    .Map(Column.G, winLoss => winLoss.SuperNetWinLoss.Total)
////                    .Map(Column.H, winLoss => winLoss.Company);
////            }

////            return mapper;
////        }

////        private TemplateInfo GetTemplateInfo(WinLossAnalysisExcelQuery message)
////            => new TemplateInfo(
////                GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
////                noInfomationText: Resources.NoInformationIsAvailable);

////        private string GetReporTitle(WinLossAnalysisExcelQuery message)
////            => $"{Resources.WinLossTitle} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}" +
////            $" {Resources.of2} {productService.ConcatProductNames(message.UserSelectedProductIds)}";

////        private string GetExcelFileName(WinLossAnalysisExcelQuery message, WinLossAnalysisParam winLossParam)
////           => string.Join(
////               "-",
////               Resources.WinLossTitle,
////               message.CustName,
////               winLossParam.FromDate.ToShortDateWithSlash(),
////               winLossParam.ToDate.ToShortDateWithSlash());
////    }
////}