namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System;
    using System.Collections.Generic;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Reports.Models.Winloss;
    using Requests.WinLoss;
    using Services;
    using ViewModels;

    public class EnteredSportResultExcelHandler :
        BaseHandler,
        IRequestHandler<EnteredSportResultExcelQuery, ExcelFileViewModel>
    {
        private readonly WinLossService reportService;
        private readonly Workbook<EnteredSportResult> workbook;

        public EnteredSportResultExcelHandler(
         WinLossService reportService,
         Workbook<EnteredSportResult> workbook)
        {
            this.reportService = reportService;
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(EnteredSportResultExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var reportData = reportService.GetEnteredSportResults();
            var resources = new Dictionary<string, string>
            {
                ["Yesterday"] = DateTimeManager.Now.AddDays(-1).ToShortDateWithSlash(),
                ["Today"] = DateTimeManager.Now.ToShortDateWithSlash()
            };

            var result = workbook
               .UseTemplate(GetTemplateInfo())
               .Header(key => resources.ContainsKey(key) ? resources[key] : Resources.ResourceManager.GetString(key))
               .Body(reportData, GetEnteredSportResultRowMapper())
               .Sheet(Resources.DefaultSheetName)
               .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(), result);
        }

        private Mapper<EnteredSportResult> GetEnteredSportResultRowMapper()
        {
            var mapper = new Mapper<EnteredSportResult>()
                .Map(Column.A, enteredSportResult => enteredSportResult.SportName)
                .Map(Column.B, enteredSportResult => enteredSportResult.EnteredSportYesterday?.Entered ?? 0)
                .Map(Column.C, enteredSportResult => enteredSportResult.EnteredSportYesterday?.NonEntered ?? 0)
                .Map(Column.D, enteredSportResult => enteredSportResult.EnteredSportToday?.Entered ?? 0)
                .Map(Column.E, enteredSportResult => enteredSportResult.EnteredSportToday?.NonEntered ?? 0);

            return mapper;
        }

        private TemplateInfo GetTemplateInfo()
            => new TemplateInfo(
                templatePath: GetExcelTemplatePath("MatchResultEntered"),
                noInfomationText: Resources.NoInformationIsAvailable,
                titleStartIndex: 0,
                headerStartIndex: 1,
                bodyStartIndex: 3,
                bodyEndIndex: 4);

        private string GetExcelFileName()
          => string.Join(
              "-",
              Resources.MatchResultEntered,
              DateTimeManager.Now.ToShortDateWithSlash(),
              DateTimeManager.Now.AddDays(-1).ToShortDateWithSlash());
    }
}