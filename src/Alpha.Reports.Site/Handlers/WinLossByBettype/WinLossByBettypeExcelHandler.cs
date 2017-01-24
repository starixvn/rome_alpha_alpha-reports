namespace Alpha.Reports.Site.Handlers.WinLossByBettype
{
    using System;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Models.WinlossByBettype;
    using Requests.WinLoss;
    using Services;
    using ViewModels;

    public class WinLossByBettypeExcelHandler :
        WinLossByBettypeBaseHandler,
        IRequestHandler<WinLossByBettypeExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<WinlossByBettypeModel> workbook;

        public WinLossByBettypeExcelHandler(
            WinLossByBettypeService reportService,
            Workbook<WinlossByBettypeModel> workbook) : base(reportService)
        {
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(WinLossByBettypeExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var winLossParam = CreateWinLossByBettypeParamater(message);
            var reportData = reportService.GetWinLossByBettype(winLossParam);
            var grandTotal = AccumulateGrandTotal(reportData);

            var result = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Title(GetReporTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData, GetBodyRowMapper(message))
                .Footer(grandTotal, GetTotalRowMapper(message))
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, winLossParam), result);
        }

        private Mapper<WinlossByBettypeModel> GetBodyRowMapper(WinLossByBettypeExcelQuery message)
          => GetRowMapper(message, isTotal: false);

        private Mapper<WinlossByBettypeModel> GetTotalRowMapper(WinLossByBettypeExcelQuery message)
            => GetRowMapper(message, isTotal: true);

        private Mapper<WinlossByBettypeModel> GetRowMapper(WinLossByBettypeExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<WinlossByBettypeModel>()
                .Map(Column.A, winLoss => isTotal ? Resources.Total : winLoss.CustName)
                .Map(Column.B, winLoss => winLoss.Turnover)
                .Map(Column.C, winLoss => winLoss.GrossComm);

            if (message.CustLevel.IsAgentLevel())
            {
                mapper.Map(Column.D, winLoss => winLoss.MemberNetWinLoss.WinLoss)
                 .Map(Column.E, winLoss => winLoss.MemberNetWinLoss.Comm)
                 .Map(Column.F, winLoss => winLoss.MemberNetWinLoss.Total)
                 .Map(Column.G, winLoss => winLoss.AgentNetWinLoss.WinLoss)
                 .Map(Column.H, winLoss => winLoss.AgentNetWinLoss.Comm)
                 .Map(Column.I, winLoss => winLoss.AgentNetWinLoss.Total)
                 .Map(Column.J, winLoss => winLoss.Company);
            }

            if (message.CustLevel.IsMasterLevel())
            {
                mapper.Map(Column.D, winLoss => winLoss.AgentNetWinLoss.Total)
                   .Map(Column.E, winLoss => winLoss.MasterNetWinLoss.WinLoss)
                   .Map(Column.F, winLoss => winLoss.MasterNetWinLoss.Comm)
                   .Map(Column.G, winLoss => winLoss.MasterNetWinLoss.Total)
                   .Map(Column.H, winLoss => winLoss.Company);
            }

            if (message.CustLevel.IsSuperLevel())
            {
                mapper.Map(Column.D, winLoss => winLoss.MasterNetWinLoss.Total)
                    .Map(Column.E, winLoss => winLoss.SuperNetWinLoss.WinLoss)
                    .Map(Column.F, winLoss => winLoss.SuperNetWinLoss.Comm)
                    .Map(Column.G, winLoss => winLoss.SuperNetWinLoss.Total)
                    .Map(Column.H, winLoss => winLoss.Company);
            }

            return mapper;
        }

        private TemplateInfo GetTemplateInfo(WinLossByBettypeExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable);

        private string GetReporTitle(WinLossByBettypeExcelQuery message)
            => $"{Resources.WinLossTitle} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}";

        private string GetExcelFileName(WinLossByBettypeExcelQuery message, WinLossByBettypeParam winLossParam)
           => string.Join(
               "-",
               Resources.WinLossTitle,
               message.CustName,
               winLossParam.FromDate.ToShortDateWithSlash(),
               winLossParam.ToDate.ToShortDateWithSlash());
    }
}