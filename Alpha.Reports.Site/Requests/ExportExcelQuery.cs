namespace Alpha.Reports.Site.Requests
{
    using Models;

    public class ExportExcelQuery<T> : BaseReportQuery
        where T : class
    {
        public string TemplatePath { get; set; }

        public string Tilte { get; set; }

        public ReportData<T> ReportData { get; set; }
    }
}