namespace Alpha.Reports.Site.Handlers.WinLossByBetType
{
    using System;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Models.WinlossByBettype;
    using Requests.WinLossByBettype;
    using Services;
    using ViewModels;

    public class WinLossByBetTypeExcelHandler :
        WinLossByBetTypeBaseHandler,
        IRequestHandler<WinLossByBettypeExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<WinLossByBetTypeModel> workbook;

        public WinLossByBetTypeExcelHandler(
            WinLossByBetTypeService reportService,
            Workbook<WinLossByBetTypeModel> workbook) : base(reportService)
        {
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(WinLossByBettypeExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var winLossByBetTypeParam = CreateWinLossByBettypeParamater(message);
            var reportData = reportService.GetWinLossByBetType(winLossByBetTypeParam);
            var grandTotal = AccumulateGrandTotal(reportData);

            var result = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Title(GetReporTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData, GetBodyRowMapper(message))
                .Footer(grandTotal, GetTotalRowMapper(message))
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, winLossByBetTypeParam), result);
        }

        private Mapper<WinLossByBetTypeModel> GetBodyRowMapper(WinLossByBettypeExcelQuery message)
          => GetRowMapper(message, isTotal: false);

        private Mapper<WinLossByBetTypeModel> GetTotalRowMapper(WinLossByBettypeExcelQuery message)
            => GetRowMapper(message, isTotal: true);

        private Mapper<WinLossByBetTypeModel> GetRowMapper(WinLossByBettypeExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<WinLossByBetTypeModel>()
                .Map(Column.A, data => isTotal ? Resources.Total : data.BetTypeName)
                .Map(Column.B, data => data.BetCount)
                .Map(Column.C, data => data.Turnover)
                .Map(Column.D, data => data.GrossComm);

            if (message.CustLevel.IsAgentLevel())
            {
                mapper.Map(Column.E, data => data.MemberNetWinLoss.WinLoss)
                 .Map(Column.F, data => data.MemberNetWinLoss.Comm)
                 .Map(Column.G, data => data.MemberNetWinLoss.Total)
                 .Map(Column.H, data => data.AgentNetWinLoss.WinLoss)
                 .Map(Column.I, data => data.AgentNetWinLoss.Comm)
                 .Map(Column.J, data => data.AgentNetWinLoss.Total)
                 .Map(Column.K, data => data.Company);
            }

            if (message.CustLevel.IsMasterLevel())
            {
                mapper.Map(Column.E, data => data.AgentNetWinLoss.Total)
                   .Map(Column.F, data => data.MasterNetWinLoss.WinLoss)
                   .Map(Column.G, data => data.MasterNetWinLoss.Comm)
                   .Map(Column.H, data => data.MasterNetWinLoss.Total)
                   .Map(Column.I, data => data.Company);
            }

            if (message.CustLevel.IsSuperLevel())
            {
                mapper.Map(Column.E, data => data.MasterNetWinLoss.Total)
                    .Map(Column.F, data => data.SuperNetWinLoss.WinLoss)
                    .Map(Column.G, data => data.SuperNetWinLoss.Comm)
                    .Map(Column.H, data => data.SuperNetWinLoss.Total)
                    .Map(Column.I, data => data.Company);
            }

            return mapper;
        }

        private TemplateInfo GetTemplateInfo(WinLossByBettypeExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable);

        private string GetReporTitle(WinLossByBettypeExcelQuery message)
            => $"{Resources.WinLossTitle} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}";

        private string GetExcelFileName(WinLossByBettypeExcelQuery message, WinLossByBetTypeParam winLossByBetTypeParam)
           => string.Join(
               "-",
               Resources.WinLossTitle,
               message.CustName,
               winLossByBetTypeParam.FromDate.ToShortDateWithSlash(),
               winLossByBetTypeParam.ToDate.ToShortDateWithSlash());
    }
}