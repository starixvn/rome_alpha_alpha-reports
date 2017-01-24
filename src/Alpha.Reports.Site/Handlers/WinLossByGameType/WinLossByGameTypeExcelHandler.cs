namespace Alpha.Reports.Site.Handlers.WinLossByGameType
{
    using System;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Models.WinlossByGameType;
    using Requests.WinLossByGameType;
    using Services;
    using ViewModels;

    public class WinLossByGameTypeExcelHandler :
        WinLossByGameTypeBaseHandler,
        IRequestHandler<WinLossByGameTypeExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<WinLossByGameTypeModel> workbook;

        public WinLossByGameTypeExcelHandler(
            WinLossByGameTypeService reportService,
            Workbook<WinLossByGameTypeModel> workbook) : base(reportService)
        {
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(WinLossByGameTypeExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var winLossByGameTypeParam = CreateWinLossByGameTypeParamater(message);
            var reportData = reportService.GetWinLossByGameType(winLossByGameTypeParam);
            var grandTotal = AccumulateGrandTotal(reportData);

            var result = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Title(GetReporTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData, GetBodyRowMapper(message))
                .Footer(grandTotal, GetTotalRowMapper(message))
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, winLossByGameTypeParam), result);
        }

        private Mapper<WinLossByGameTypeModel> GetBodyRowMapper(WinLossByGameTypeExcelQuery message)
          => GetRowMapper(message, isTotal: false);

        private Mapper<WinLossByGameTypeModel> GetTotalRowMapper(WinLossByGameTypeExcelQuery message)
            => GetRowMapper(message, isTotal: true);

        private Mapper<WinLossByGameTypeModel> GetRowMapper(WinLossByGameTypeExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<WinLossByGameTypeModel>()
                .Map(Column.A, data => isTotal ? Resources.Total : data.GameTypeName)
                .Map(Column.B, data => data.Turnover);

            if (message.CustLevel.IsAgentLevel())
            {
                mapper.Map(Column.C, data => data.MemberNetWinLoss.WinLoss)
                 .Map(Column.D, data => data.MemberNetWinLoss.Comm)
                 .Map(Column.E, data => data.MemberNetWinLoss.Total)
                 .Map(Column.F, data => data.AgentNetWinLoss.WinLoss)
                 .Map(Column.G, data => data.AgentNetWinLoss.Comm)
                 .Map(Column.H, data => data.AgentNetWinLoss.Total)
                 .Map(Column.I, data => data.Company);
            }

            if (message.CustLevel.IsMasterLevel())
            {
                mapper.Map(Column.C, data => data.AgentNetWinLoss.Total)
                   .Map(Column.D, data => data.MasterNetWinLoss.WinLoss)
                   .Map(Column.E, data => data.MasterNetWinLoss.Comm)
                   .Map(Column.F, data => data.MasterNetWinLoss.Total)
                   .Map(Column.G, data => data.Company);
            }

            if (message.CustLevel.IsSuperLevel())
            {
                mapper.Map(Column.C, data => data.MasterNetWinLoss.Total)
                    .Map(Column.D, data => data.SuperNetWinLoss.WinLoss)
                    .Map(Column.E, data => data.SuperNetWinLoss.Comm)
                    .Map(Column.F, data => data.SuperNetWinLoss.Total)
                    .Map(Column.G, data => data.Company);
            }

            return mapper;
        }

        private TemplateInfo GetTemplateInfo(WinLossByGameTypeExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable);

        private string GetReporTitle(WinLossByGameTypeExcelQuery message)
            => $"{Resources.WinLossTitle} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}";

        private string GetExcelFileName(WinLossByGameTypeExcelQuery message, WinLossByGameTypeParam winLossByGameTypeParam)
           => string.Join(
               "-",
               Resources.WinLossTitle,
               message.CustName,
               winLossByGameTypeParam.FromDate.ToShortDateWithSlash(),
               winLossByGameTypeParam.ToDate.ToShortDateWithSlash());
    }
}