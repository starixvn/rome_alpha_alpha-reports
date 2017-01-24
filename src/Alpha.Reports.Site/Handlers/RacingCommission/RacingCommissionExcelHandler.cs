namespace Alpha.Reports.Site.Handlers.RacingCommission
{
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Models.RacingCommission;
    using Requests.RacingCommission;
    using Services;
    using ViewModels;

    public class RacingCommissionExcelHandler : BaseHandler, IRequestHandler<RacingCommissionExcelQuery, ExcelFileViewModel>
    {
        private readonly RacingCommissionService racingCommissionService;
        private readonly Workbook<RacingCommissionModel> workbook;

        public RacingCommissionExcelHandler(RacingCommissionService racingCommissionService, Workbook<RacingCommissionModel> workbook)
        {
            this.racingCommissionService = racingCommissionService;
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(RacingCommissionExcelQuery message)
        {
            var param = new RacingCommissionParam(message.ViewerLevel, message.CustLevel, message.CustId, message.FromDate, message.ToDate);
            var commissions = racingCommissionService.GetCustomerCommission(param);

            var result = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Title(GetReporTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(commissions, GetBodyRowMapper())
                .Footer(commissions, GetTotalRowMapper())
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, param), result);
        }

        private Mapper<RacingCommissionModel> GetBodyRowMapper() => GetRowMapper(false);

        private Mapper<RacingCommissionModel> GetTotalRowMapper() => GetRowMapper(true);

        private Mapper<RacingCommissionModel> GetRowMapper(bool isTotal)
        {
            var mapper = new Mapper<RacingCommissionModel>()
                .Map(Column.A, commission => isTotal ? Resources.Total : commission.CustName)
                .Map(Column.B, commission => commission.ToteWin)
                .Map(Column.C, commission => commission.TotePlace)
                .Map(Column.D, commission => commission.ToteWinPlace)
                .Map(Column.E, commission => commission.Total);

            return mapper;
        }

        private TemplateInfo GetTemplateInfo(RacingCommissionExcelQuery message) => new TemplateInfo(
               GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
               noInfomationText: Resources.NoInformationIsAvailable);

        private string GetReporTitle(RacingCommissionExcelQuery message)
            => $"{Resources.RacingCommission} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}";

        private string GetExcelFileName(RacingCommissionExcelQuery message, RacingCommissionParam winLossParam)
            => string.Join("-", Resources.RacingCommission, message.CustName, winLossParam.FromDate.ToShortDateWithSlash(), winLossParam.ToDate.ToShortDateWithSlash());
    }
}