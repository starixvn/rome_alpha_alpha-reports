namespace Alpha.Reports.Site.Handlers.PlayForFun
{
    using System.Linq;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Models.PlayForFun;
    using Requests.PlayForFun;
    using Services;
    using ViewModels;
    using static ONEbook.SpreadSheets.Column;

    public class PlayForFunExcelHandler :
        PlayForFunBaseHandler,
        IRequestHandler<MasterOrAgentPlayForFunExcelQuery, ExcelFileViewModel>,
        IRequestHandler<MemberPlayForFunExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<PlayForFunModel> workbook;

        public PlayForFunExcelHandler(
            ProductService productService,
            PlayForFunService reportService,
            Workbook<PlayForFunModel> workbook)
            : base(productService, reportService)
        {
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(MasterOrAgentPlayForFunExcelQuery message)
        {
            var products = reportService.SupportedProduct(message.CustId).ToList();
            var param = CreatePlayForFunParam(message, products);
            var reportData = reportService.MasterOrAgentPlayForFun(param);

            var fileContent = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Title(GetReportTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData, GetMasterAgentBodyRowMapper())
                .Footer(BuildMasterPlayForFunTotal(reportData), GetMasterAgentTotalRowMapper())
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, param), fileContent);
        }

        public ExcelFileViewModel Handle(MemberPlayForFunExcelQuery message)
        {
            var products = reportService.SupportedProduct(message.CustId).ToList();
            var param = CreatePlayForFunParam(message, products);
            var reportData = reportService.MemberPlayForFun(param);

            var fileContent = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Title(GetReportTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData, GetMemberBodyRowMapper())
                .Footer(BuildMemberPlayForFunTotal(reportData), GetMemberTotalRowMapper())
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, param), fileContent);
        }

        private TemplateInfo GetTemplateInfo(PlayForFunQuery message)
              => new TemplateInfo(
                  GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
                  noInfomationText: Resources.NoInformationIsAvailable,
                  titleStartIndex: 1,
                  headerStartIndex: 2,
                  bodyStartIndex: 3,
                  bodyEndIndex: 4)
              .FreezePanel(3, A);

        private string GetReportTitle(PlayForFunQuery message)
            => string.Join(
                " ",
                $"{GetReportType(message)} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}",
                Resources.of2,
                productService.ConcatProductNames(message.UserSelectedProductId.ToString()),
                $"({GetStatusName(message.UserSelectedStatusId)} {Resources.Status})");

        private string GetReportType(PlayForFunQuery message)
            => message.CustLevel.IsSuperLevel()
            ? Resources.MasterPlayForFunDetail
            : message.CustLevel.IsMasterLevel()
            ? Resources.AgentPlayForFunDetail
            : Resources.MemberPlayForFunDetail;

        private string GetExcelFileName(PlayForFunQuery message, PlayForFunParam param)
            => string.Join(
                "-",
                Resources.PlayForFun,
                message.CustName,
                param.FromDate.ToShortDateWithSlash(),
                param.ToDate.ToShortDateWithSlash(),
                productService.ConcatProductNames(message.UserSelectedProductId.ToString()));

        private Mapper<PlayForFunModel> GetMasterAgentBodyRowMapper() => GetMasterAgentRowMapper(isTotal: false);

        private Mapper<PlayForFunModel> GetMasterAgentTotalRowMapper() => GetMasterAgentRowMapper(isTotal: true);

        private Mapper<PlayForFunModel> GetMasterAgentRowMapper(bool isTotal)
        {
            var mapper = new Mapper<PlayForFunModel>()
                .Map(A, model => isTotal ? Resources.Total : model.CustName)
                .Map(B, model => ((MasterAgentPlayForFunModel)model).NumberOfPlayers)
                .Map(C, model => model.Turnover)
                .Map(D, model => model.WinLoss)
                .Map(E, model => model.BetCount);

            return mapper;
        }

        private Mapper<PlayForFunModel> GetMemberBodyRowMapper() => GetMemberRowMapper(isTotal: false);

        private Mapper<PlayForFunModel> GetMemberTotalRowMapper() => GetMemberRowMapper(isTotal: true);

        private Mapper<PlayForFunModel> GetMemberRowMapper(bool isTotal)
        {
            var mapper = new Mapper<PlayForFunModel>()
                .Map(A, model => isTotal ? Resources.Total : model.CustName)
                .Map(B, model => GetStatusName(((MemberPlayForFunModel)model).StatusId))
                .Map(C, model => model.Turnover)
                .Map(D, model => model.WinLoss)
                .Map(E, model => model.BetCount);

            return mapper;
        }
    }
}