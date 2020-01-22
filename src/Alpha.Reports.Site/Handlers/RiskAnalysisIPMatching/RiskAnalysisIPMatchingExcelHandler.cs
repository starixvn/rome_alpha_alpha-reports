namespace Alpha.Reports.Site.Handlers.RiskAnalysisIPMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Requests.RiskAnalysisIPMatching;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params;
    using Services;
    using System;
    using ViewModels;
    using static ONEbook.SpreadSheets.Column;

    public class RiskAnalysisIPMatchingExcelHandler :
        BaseHandler,
        IRequestHandler<RiskAnalysisIPMatchingExcelQuery, ExcelFileViewModel>
    {
        protected readonly RiskAnalysisIPMatchingService riskAnalysisIPMatchingService;
        private readonly Workbook<RiskAnalysisIPMatchingModel> workbook;

        public RiskAnalysisIPMatchingExcelHandler(
            RiskAnalysisIPMatchingService riskAnalysisIPMatchingService,
            Workbook<RiskAnalysisIPMatchingModel> workbook)
        {
            this.riskAnalysisIPMatchingService = riskAnalysisIPMatchingService;
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(RiskAnalysisIPMatchingExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var viewModel = new RiskAnalysisIPMatchingViewModel(
                 message.SelectedWinlostDate,
                 message.MinDate,
                 message.MemberCount,
                 riskAnalysisIPMatchingService.GetRiskAnalysisIPMatchingModel(
                     new RiskAnalysisIPMatchingParam(
                         message.CustId, message.SelectedWinlostDate, message.MoveBetsDate, message.MemberCount)));

            var result = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Title(GetReportTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(viewModel.RiskAnalysisIPMatchingItems, GetBodyRowMapper(message))
                .Footer(viewModel.TotalRow, GetTotalRowMapper(message))
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, message.SelectedWinlostDate), result);
        }

        private Mapper<RiskAnalysisIPMatchingModel> GetBodyRowMapper(RiskAnalysisIPMatchingExcelQuery message)
          => GetRowMapper(message, isTotal: false);

        private Mapper<RiskAnalysisIPMatchingModel> GetTotalRowMapper(RiskAnalysisIPMatchingExcelQuery message)
            => GetRowMapper(message, isTotal: true);

        internal Mapper<RiskAnalysisIPMatchingModel> GetRowMapper(RiskAnalysisIPMatchingExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<RiskAnalysisIPMatchingModel>()
                .Map(Column.A, data => isTotal ? Resources.Total : data.IP)
                .Map(Column.B, data => data.MemberCount)
                .Map(Column.C, data => data.BetCount)
                .Map(Column.D, data => data.MemberTurnover)
                .Map(Column.E, data => data.MemberWinLoss)
                .Map(Column.F, data => data.SuperWinLoss)
                .Map(Column.G, data => data.Company);

            return mapper;
        }

        private TemplateInfo GetTemplateInfo(RiskAnalysisIPMatchingExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath("Super"),
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

        private string GetReportTitle(RiskAnalysisIPMatchingExcelQuery message)
            => $"{Resources.IPMatching} {message.SelectedWinlostDate.ToShortDateWithSlash()}";

        private string GetExcelFileName(RiskAnalysisIPMatchingExcelQuery message, DateTime winlostDate)
           => string.Join(
               "-",
               Resources.IPMatching,
               message.CustName,
               winlostDate.ToShortDateWithSlash());
    }
}