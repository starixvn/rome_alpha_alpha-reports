namespace Alpha.Reports.Site.Handlers.WinLossByProduct
{
    using System;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params;
    using RemotingRef;
    using Reports.Models.WinLossByProduct;
    using Requests.WinLossByProduct;
    using Services;
    using ViewModels;

    public class WinLossByProductExcelHandler :
        WinLossByProductBaseHandler,
        IRequestHandler<WinLossByProductExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<WinLossByProductModel> workbook;

        public WinLossByProductExcelHandler(
            WinLossByProductService reportService,
            CacheRefService cacheRefService,
            Workbook<WinLossByProductModel> workbook) : base(reportService, cacheRefService)
        {
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(WinLossByProductExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var winLossByProductParam = CreateWinLossByProductParamater(message);
            var reportData = reportService.GetWinLossByProduct(winLossByProductParam);
            var grandTotal = AccumulateGrandTotal(reportData);

            var result = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Title(GetReporTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData, GetBodyRowMapper(message))
                .Footer(grandTotal, GetTotalRowMapper(message))
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, winLossByProductParam), result);
        }

        private Mapper<WinLossByProductModel> GetBodyRowMapper(WinLossByProductExcelQuery message)
          => GetRowMapper(message, isTotal: false);

        private Mapper<WinLossByProductModel> GetTotalRowMapper(WinLossByProductExcelQuery message)
            => GetRowMapper(message, isTotal: true);

        private Mapper<WinLossByProductModel> GetRowMapper(WinLossByProductExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<WinLossByProductModel>()
                .Map(Column.A, data => isTotal ? Resources.Total : data.ProductName)
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

        private TemplateInfo GetTemplateInfo(WinLossByProductExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable);

        private string GetReporTitle(WinLossByProductExcelQuery message)
            => $"{Resources.WinLossByProduct} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}";

        private string GetExcelFileName(WinLossByProductExcelQuery message, WinLossByProductParam winLossByProductParam)
           => string.Join(
               "-",
               Resources.WinLossByProduct,
               message.CustName,
               winLossByProductParam.FromDate.ToShortDateWithSlash(),
               winLossByProductParam.ToDate.ToShortDateWithSlash());
    }
}