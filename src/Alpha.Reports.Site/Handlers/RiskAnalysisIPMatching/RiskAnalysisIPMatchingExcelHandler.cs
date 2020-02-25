namespace Alpha.Reports.Site.Handlers.RiskAnalysisIPMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Requests.RiskAnalysisIPMatching;
    using Alpha.Utilities.IP;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Services;
    using System;
    using ViewModels;
    using static ONEbook.SpreadSheets.Column;

    public class RiskAnalysisIPMatchingExcelHandler :
        RiskAnalysisIPMatchingBaseHandler, 
        IRequestHandler<RiskAnalysisIPMatchingExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<RiskAnalysisIPMatchingModel> workbook;

        public RiskAnalysisIPMatchingExcelHandler(
            RiskAnalysisIPMatchingService riskAnalysisIPMatchingService,
            IPService ipService,
            Workbook<RiskAnalysisIPMatchingModel> workbook) 
            : base(riskAnalysisIPMatchingService, ipService)
        {
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(RiskAnalysisIPMatchingExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var viewModel = BuildRiskAnalysisIPMatchingViewModel(message);

            var result = workbook
                .UseTemplate(GetTemplateInfo())
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
                .Map(A, data => isTotal ? Resources.Total : data.IP)
                .Map(B, data => data.MemberCount)
                .Map(C, data => data.BetCount)
                .Map(D, data => data.Stake)
                .Map(E, data => data.MemberWinLoss)
                .Map(F, data => data.SuperWinLoss)
                .Map(G, data => data.Company);

            return mapper;
        }

#pragma warning disable S109 // Magic numbers should not be used
        private TemplateInfo GetTemplateInfo()
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
#pragma warning restore S109 // Magic numbers should not be used

        private string GetReportTitle(RiskAnalysisIPMatchingExcelQuery message)
            => $"{Resources.IPMatching} {message.SelectedWinlostDate.ToShortDateWithSlash()} - {Resources.MemberCount}: >= {message.MemberCount}";

        private string GetExcelFileName(RiskAnalysisIPMatchingExcelQuery message, DateTime winlostDate)
           => string.Join(
               "-",
               Resources.IPMatching,
               message.CustName,
               winlostDate.ToShortDateWithSlash());
    }
}