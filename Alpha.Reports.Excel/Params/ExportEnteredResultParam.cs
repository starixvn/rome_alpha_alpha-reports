namespace Alpha.Reports.Excel.Params
{
    using System.Collections.Generic;
    using Models.Winloss;

    public class ExportEnteredResultParam
    {
        public ExportEnteredResultParam(
            string templatePath,
            string reportName,
            string sheetName,
            IEnumerable<EnteredSportResult> reportData)
        {
            TemplatePath = templatePath;
            ReportName = reportName;
            SheetName = sheetName;
            ReportData = reportData;
        }

        public string TemplatePath { get;}

        public string ReportName { get;}

        public string SheetName { get;}

        public IEnumerable<EnteredSportResult> ReportData { get;}
    }
}
