namespace Alpha.Reports.Site.Handlers.WinLossByBetType
{
    using System;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params;
    using RemotingRef;
    using Reports.Models.WinlossByBetType;
    using Requests.WinLossByBetType;
    using Services;
    using ViewModels;

    public class WinLossByBetTypeExcelHandler :
        WinLossByBetTypeBaseHandler,
        IRequestHandler<WinLossByBetTypeExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<WinLossByBetTypeModel> workbook;

        public WinLossByBetTypeExcelHandler(
            WinLossByBetTypeService reportService,
            CacheRefService cacheRefService,
            Workbook<WinLossByBetTypeModel> workbook) : base(reportService, cacheRefService)
        {
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(WinLossByBetTypeExcelQuery message)
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

        private Mapper<WinLossByBetTypeModel> GetBodyRowMapper(WinLossByBetTypeExcelQuery message)
          => GetRowMapper(message, isTotal: false);

        private Mapper<WinLossByBetTypeModel> GetTotalRowMapper(WinLossByBetTypeExcelQuery message)
            => GetRowMapper(message, isTotal: true);

        private Mapper<WinLossByBetTypeModel> GetRowMapper(WinLossByBetTypeExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<WinLossByBetTypeModel>()
                .Map(Column.A, data => isTotal ? Resources.Total : BuildDisplayedBetTypeName(data.BetTypeId, data.BetTypeName, data.LiveIndicator))
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

        private TemplateInfo GetTemplateInfo(WinLossByBetTypeExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable);

        private string GetReporTitle(WinLossByBetTypeExcelQuery message)
            => $"{Resources.WinLossByBetType} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}";

        private string GetExcelFileName(WinLossByBetTypeExcelQuery message, WinLossByBetTypeParam winLossByBetTypeParam)
           => string.Join(
               "-",
               Resources.WinLossByBetType,
               message.CustName,
               winLossByBetTypeParam.FromDate.ToShortDateWithSlash(),
               winLossByBetTypeParam.ToDate.ToShortDateWithSlash());
    }
}