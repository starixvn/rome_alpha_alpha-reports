namespace Alpha.Reports.Site.Requests
{
    using Models;

    public class ExportExcelQuery<T> : BaseReportQuery
        where T : class
    {
        public string TemplatePath { get; set; }

        public string Title { get; set; }

        public int MaxColNumber { get; set; }

        public string SheetName { get; set; }

        public ReportData<T> ReportData { get; set; }

        public int DataLength { get; set; }
    }
}