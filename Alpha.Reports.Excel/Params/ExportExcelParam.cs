namespace Alpha.Reports.Excel.Params
{
    using System;
    using Models;

    public class ExportExcelParam<T> where T : class
    {
        public string TemplatePath { get; set; }

        public string ReportName { get; set; }

        public string SheetName { get; set; }

        public ReportData<T> ReportData { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
