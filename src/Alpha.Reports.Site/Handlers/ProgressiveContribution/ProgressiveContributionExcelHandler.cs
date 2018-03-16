namespace Alpha.Reports.Site.Handlers.ProgressiveContribution
{
    using System;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Models.ProgressiveContribution;
    using Requests.ProgressiveContribution;
    using Services;
    using ViewModels;
    using static ONEbook.SpreadSheets.Column;

    public class ProgressiveContributionExcelHandler :
        ProgressiveContributionBaseHandler,
        IRequestHandler<ProgressiveContributionExcelQuery, ExcelFileViewModel>
    {
        private readonly string betTypeName = nameof(ProgressiveContributionByBetTypeModel.BetTypeName);
        private readonly ProgressiveContributionWorkbook workbook;

        public ProgressiveContributionExcelHandler(
            ProductService productService,
            ProgressiveContributionService progressiveContributionService,
            ProgressiveContributionWorkbook workbook)
            : base(productService, progressiveContributionService)
        {
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(ProgressiveContributionExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var param = CreateProgressiveContributionParam(message);
            var reportData = progressiveContributionService.GetProgressiveContributions(param);
            var grandTotal = AccumulateGrandTotal(reportData);

            var fileContent = workbook.
                SetData(reportData, grandTotal)
                .SetRowMappers(
                    GetBodyRowMapper(message),
                    GetTotalRowMapper(message))
                .UseTemplate(GetTemplateInfo(message))
                .Sheet(Resources.DefaultSheetName)
                .Title(GetReporTitle(message))
                .Header(key => key.Equals(betTypeName) ? $"{Resources.Product}/{Resources.BetType}" : Resources.ResourceManager.GetString(key))
                .Footer(grandTotal, GetTotalRowMapper(message))
                .Title(GetReporTitle(message))
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, param), fileContent);
        }

        private Mapper<ProgressiveContributionByBetTypeModel> GetBodyRowMapper(ProgressiveContributionExcelQuery message)
          => GetRowMapper(message, isTotal: false);

        private Mapper<ProgressiveContributionByBetTypeModel> GetTotalRowMapper(ProgressiveContributionExcelQuery message)
            => GetRowMapper(message, isTotal: true);

        internal Mapper<ProgressiveContributionByBetTypeModel> GetRowMapper(ProgressiveContributionExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<ProgressiveContributionByBetTypeModel>()
                .Map(A, data => isTotal ? Resources.Total : data.BetTypeName)
                .Map(B, data => data.Turnover)
                .Map(C, data => data.ContributionAmount)
                .Map(D, data => data.AgentContribution);

            if (message.CustLevel.IsAgentLevel())
            {
                mapper.Map(E, data => data.CompanyContribution);
            }
            else if (message.CustLevel.IsMasterLevel())
            {
                mapper.Map(E, data => data.MasterContribution)
                    .Map(F, data => data.CompanyContribution);
            }
            else
            {
                mapper.Map(E, data => data.MasterContribution)
                    .Map(F, data => data.SuperContribution)
                    .Map(G, data => data.CompanyContribution);
            }

            return mapper;
        }

        private TemplateInfo GetTemplateInfo(ProgressiveContributionExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable,
                titleStartIndex: 1,
                headerStartIndex: 2,
                bodyStartIndex: 4,
                bodyEndIndex: 11,
                freezeRowIndex: 0,
                freezeColumnName: A,
                columnStartName: A,
                sheetIndex: 1,
                footerEndIndex: 12);

        private string GetReporTitle(ProgressiveContributionExcelQuery message)
           => $"{Resources.ProgressiveGamesContribution} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}" +
            $" {Resources.of2} {productService.ConcatProductNames(message.UserSelectedProductIds)}";

        private static string GetExcelFileName(ProgressiveContributionExcelQuery message, ProgressiveContributionParam param)
           => string.Join(
               "-",
               Resources.ProgressiveGamesContribution,
               message.CustName,
               param.FromDate.ToShortDateWithSlash(),
               param.ToDate.ToShortDateWithSlash());
    }
}