namespace Alpha.Reports.Site.Handlers.BetChoiceMatching
{
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Params.BetChoiceMatching;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Requests.BetChoiceMatching;
    using Alpha.Reports.Site.ViewModels;
    using EnumsNET;
    using MediatR;
    using ONEbook.SpreadSheets;
    using System.Collections.Generic;

    public class BetChoiceMatchingExcelHandler :
        BaseHandler,
        IRequestHandler<BetChoiceMatchingExcelQuery, ExcelFileViewModel>
    {
        private readonly BetChoiceMatchingService betChoiceMatchingService;
        private readonly BetChoiceMatchingWorkbook workbook;

        public BetChoiceMatchingExcelHandler(BetChoiceMatchingService betChoiceMatchingService,
            BetChoiceMatchingWorkbook workbook)
        {
            this.betChoiceMatchingService = betChoiceMatchingService;
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(BetChoiceMatchingExcelQuery message)
        {
            var param = new BetChoiceMatchingExcelParam(
                message.CustId,
                message.WinlostDate,
                message.MoveBetsDate,
                message.BetCount,
                message.Username,
                message.Stake,
                message.MemberWinLossOperator,
                message.MemberWinLossValue);
            var events = betChoiceMatchingService.ExcelQuery(param);
            var fileContent = workbook
                .SetData(events)
                .UseTemplate(GetTemplateInfo(message))
                .Sheet(Resources.DefaultSheetName)
                .Title(GetReportTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .ExportToBytes();
            return new ExcelFileViewModel(GetExcelFileName(message), fileContent);
        }

        private TemplateInfo GetTemplateInfo(BetChoiceMatchingExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable,
                columnStartName: Column.A);

        private static string GetReportTitle(BetChoiceMatchingExcelQuery message)
        {
            var filters = new List<string>
            {
                $"{Resources.MatchingBets} >= {message.BetCount}"
            };

            if (message.Stake.HasValue)
            {
                filters.Add($"{Resources.TotalStake} >= {string.Format("{0:n0}", message.Stake.Value)}");
            }

            if (message.MemberWinLossValue.HasValue)
            {
                filters.Add($"{Resources.TotalMemberWL} {message.MemberWinLossOperator.AsString(EnumFormat.Description)} {message.MemberWinLossValue.Value.FormatNumber()}");
            }

            if (!string.IsNullOrEmpty(message.Username))
            {
                filters.Add($"{Resources.Username}: {message.Username}");
            }

            return $"{Resources.BetChoiceMatching} {message.WinlostDate.ToShortDateWithSlash()} - {string.Join("; ", filters)}";
        }

        private static string GetExcelFileName(BetChoiceMatchingExcelQuery message)
            => string.Join(
                "-",
                Resources.BetChoiceMatching,
                message.CustName,
                message.WinlostDate.ToShortDateWithSlash());
    }
}