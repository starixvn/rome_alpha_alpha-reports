namespace Alpha.Reports.Site.Handlers.CommissionByBettype
{
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Models.CommissionByBettype;
    using Requests.CommissionByBettype;
    using Services;
    using ViewModels;

    public class CommissionByBettypeExcelHandler : BaseHandler, IRequestHandler<CommissionByBettypeExcelQuery, ExcelFileViewModel>
    {
        private readonly CommissionByBettypeService commissionService;
        private readonly ReportDateService reportDateService;
        private readonly CommissionByBettypeWorkbook workbook;

        public CommissionByBettypeExcelHandler(
            CommissionByBettypeService commissionServiceImpl,
            ReportDateService reportDateServiceImpl,
            CommissionByBettypeWorkbook workbookImpl)
        {
            commissionService = commissionServiceImpl;
            reportDateService = reportDateServiceImpl;
            workbook = workbookImpl;
        }

        public ExcelFileViewModel Handle(CommissionByBettypeExcelQuery message)
        {
            var param = new CommissionByBettypeParam(
                message.ViewerLevel,
                message.CustLevel,
                message.CustId,
                message.FromDate,
                message.ToDate,
                reportDateService.GetMoveBetsDay());

            var commissions = commissionService.GetAllCommissionByBettypes(param);

            var result = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Title(GetReportTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(commissions, GetBodyRowMapper())
                .Footer(CommissionByBettypeModel.GetAccumulatedCommission(commissions), GetTotalRowMapper())
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, param), result);
        }

        private Mapper<CommissionByBettypeModel> GetBodyRowMapper() => GetRowMapper(false);

        private Mapper<CommissionByBettypeModel> GetTotalRowMapper() => GetRowMapper(true);

        private Mapper<CommissionByBettypeModel> GetRowMapper(bool isTotal)
        {
            var mapper = new Mapper<CommissionByBettypeModel>()
            .Map(Column.A, commission => isTotal ? Resources.Total : commission.CustName)
            .Map(Column.B, commission => commission.Comm1)
            .Map(Column.C, commission => commission.Comm2)
            .Map(Column.D, commission => commission.Comm3)
            .Map(Column.E, commission => commission.ChargeComm)
            .Map(Column.F, commission => commission.MyanmarOddsComm)
            .Map(Column.G, commission => commission.CasinoComm);

            return mapper;
        }

        private TemplateInfo GetTemplateInfo(CommissionByBettypeExcelQuery message) => new TemplateInfo(
               GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
               Resources.NoInformationIsAvailable,
               bodyStartIndex: 3,
               bodyEndIndex: 4);

        private string GetReportTitle(CommissionByBettypeExcelQuery message)
            => $"{Resources.CommissionByBettype} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}";

        private string GetExcelFileName(CommissionByBettypeExcelQuery message, CommissionByBettypeParam commissionParam)
            => string.Join(
                "-",
                Resources.CommissionByBettype,
                message.CustName,
                commissionParam.FromDate.ToShortDateWithSlash(),
                commissionParam.ToDate.ToShortDateWithSlash());
    }
}