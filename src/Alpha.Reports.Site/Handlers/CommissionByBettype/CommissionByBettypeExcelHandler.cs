namespace Alpha.Reports.Site.Handlers.CommissionByBettype
{
    using Alpha.Reports.Models;
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
        private readonly Workbook<CommissionByBettypeModel> workbook;

        public CommissionByBettypeExcelHandler(
            CommissionByBettypeService commissionServiceImpl,
            ReportDateService reportDateServiceImpl,
            Workbook<CommissionByBettypeModel> workbookImpl)
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
                reportDateService.GetMoveBetsDay(),
                message.LevelType);

            var commissions = commissionService.GetAllCommissionByBettypes(param);

            var result = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Title(GetReportTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(commissions, GetBodyRowMapper(message.CustLevel))
                .Footer(CommissionByBettypeModel.GetAccumulatedCommission(commissions), GetTotalRowMapper(message.CustLevel))
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, param), result);
        }

        private Mapper<CommissionByBettypeModel> GetBodyRowMapper(UserLevel custLevel) => GetRowMapper(custLevel, false);

        private Mapper<CommissionByBettypeModel> GetTotalRowMapper(UserLevel custLevel) => GetRowMapper(custLevel, true);

        internal Mapper<CommissionByBettypeModel> GetRowMapper(UserLevel custLevel, bool isTotal)
        {
            var mapper = new Mapper<CommissionByBettypeModel>()
                .Map(Column.A, commission => isTotal ? Resources.Total : commission.CustName);
            if (custLevel.IsAgentLevel())
            {
                return mapper
                    .Map(Column.B, commission => commission.Comm1)
                    .Map(Column.C, commission => commission.Comm2)
                    .Map(Column.D, commission => commission.Comm3)
                    .Map(Column.E, commission => commission.CasinoComm);
            }

            if (!isTotal)
            {
                mapper.Map(Column.B, commission => commission.IsDirectMember
                    ? Resources.DirectMember
                    : (custLevel.IsSuperLevel() ? Resources.MasterAgent : Resources.Agent));
            }

            return mapper
                .Map(Column.C, commission => commission.Comm1)
                .Map(Column.D, commission => commission.Comm2)
                .Map(Column.E, commission => commission.Comm3)
                .Map(Column.F, commission => commission.CasinoComm);
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