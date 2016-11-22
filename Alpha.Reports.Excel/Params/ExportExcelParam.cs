namespace Alpha.Reports.Excel.Params
{
    using System;
    using Reports.Models;

    public class ExportExcelParam<T> where T : class
    {
        public string TemplatePath { get; set; }

        public string Title { get; set; }

        public string SheetName { get; set; }

        public ReportData<T> ReportData { get; set; }

        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int MaxColNumber { get; set; }
    }
}
