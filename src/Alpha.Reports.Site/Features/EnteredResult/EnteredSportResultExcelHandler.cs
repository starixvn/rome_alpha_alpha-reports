namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System;
    using System.Collections.Generic;
    using Alpha.Reports.Site.Features.EnteredResult;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Reports.Models.Winloss;
    using Requests.WinLoss;
    using Services;
    using Utils;
    using ViewModels;

    public class EnteredSportResultExcelHandler :
        EnteredSportResultBaseHandler,
        IRequestHandler<EnteredSportResultExcelQuery, ExcelFileViewModel>
    {
        private readonly EnteredSportResultWorkbook workbook;

        public EnteredSportResultExcelHandler(
             WinLossService reportService,
             EnteredSportResultWorkbook workbook) : base(reportService)
        {
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(EnteredSportResultExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var reportData = reportService.GetEnteredSportResults();
            var totalEnteredSport = GetTotalEnteredSportYesterdayAndToday(reportData);

            var resources = new Dictionary<string, string>
            {
                ["Yesterday"] = SystemTime.Today.AddDays(-1).ToShortDateWithSlash(),
                ["Today"] = SystemTime.Today.ToShortDateWithSlash()
            };

            var result = workbook
               .SetData(reportData, totalEnteredSport)
               .SetRowMappers(GetEnteredSportResultRowMapper())
               .UseTemplate(GetTemplateInfo())
               .Header(key => resources.ContainsKey(key) ? resources[key] : Resources.ResourceManager.GetString(key))
               .Sheet(Resources.DefaultSheetName)
               .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(), result);
        }

        internal Mapper<EnteredSportResult> GetEnteredSportResultRowMapper()
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
                bodyEndIndex: 5);

        private string GetExcelFileName()
          => string.Join(
              "-",
              Resources.MatchResultEntered,
              SystemTime.Today.AddDays(-1).ToShortDateWithSlash(),
              SystemTime.Today.ToShortDateWithSlash());
    }
}