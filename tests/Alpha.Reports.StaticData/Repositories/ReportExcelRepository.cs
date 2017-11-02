namespace Alpha.Reports.StaticData.Repositories
{
    using System.IO;
    using Alpha.StaticData;

    public class ReportExcelRepository : ExcelDynamicRepository
    {
        public override string GetExcelDirectory()
        {
            var rootPath = (Path.GetDirectoryName(typeof(ReportExcelRepository).Assembly.GetName().CodeBase).Substring(6));
            var excelPath = "excelData";

            return $"{rootPath}\\{excelPath}";
        }
    }
}
