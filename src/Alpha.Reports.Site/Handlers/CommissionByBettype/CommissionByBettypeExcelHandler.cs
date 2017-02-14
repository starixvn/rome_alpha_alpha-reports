namespace Alpha.Reports.Site.Handlers.CommissionByBettype
{
    using Services;
    using App_GlobalResources;
    using ONEbook.SpreadSheets;
    using Requests.CommissionByBettype;
    using ViewModels;
    using Extensions;
    using Reports.Models.CommissionByBettype;
    using Params;
    using System;
    using MediatR;

    public class CommissionByBettypeExcelHandler : BaseHandler, IRequestHandler<CommissionByBettypeExcelQuery, ExcelFileViewModel>
    {
        private readonly CommissionByBettypeService commissionService;
        private readonly Workbook<CommissionByBettypeModel> workbook;

        public CommissionByBettypeExcelHandler(CommissionByBettypeService commissionService, Workbook<CommissionByBettypeModel> workbook)
        {
            this.commissionService = commissionService;
            this.workbook = new CommissionByBettypeWorkbook(true, false, false);
        }

        public ExcelFileViewModel Handle(CommissionByBettypeExcelQuery message)
        {
            var param = new CommissionByBettypeParam(message.ViewerLevel, message.CustLevel, message.CustId, message.FromDate, message.ToDate, new DateTime());
            var commissions = commissionService.GetAllCommissionByBettypes(param);

            var result = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Title(GetReporTitle(message))
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
            .Map(Column.B, commission => commission.TotalComm1)
            .Map(Column.C, commission => commission.TotalComm2)
            .Map(Column.D, commission => commission.TotalComm3)
            .Map(Column.E, commission => commission.ChargeComm)
            .Map(Column.F, commission => commission.MOComm)
            .Map(Column.G, commission => commission.CasinoComm)
            .Map(Column.H, commission => commission.OtherComm);

            return mapper;
        }

        private TemplateInfo GetTemplateInfo(CommissionByBettypeExcelQuery message) => new TemplateInfo(
               GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
               Resources.NoInformationIsAvailable,
               bodyStartIndex: 3,
               bodyEndIndex: 4);

        private string GetReporTitle(CommissionByBettypeExcelQuery message)
            => $"{Resources.CommissionByBettype} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}";

        private string GetExcelFileName(CommissionByBettypeExcelQuery message, CommissionByBettypeParam commissionParam)
            => string.Join("-", Resources.CommissionByBettype, message.CustName, commissionParam.FromDate.ToShortDateWithSlash(), commissionParam.ToDate.ToShortDateWithSlash());
    }
}