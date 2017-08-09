using System.IO;
using Alpha.Reports.StaticData.Configurations;
using Alpha.StaticData.NPOIExtensions;
using Alpha.StaticData.Repositories;
using NPOI.SS.UserModel;

namespace Alpha.Reports.StaticData.Repositories
{
    public class ReportExcelRepository : ExcelRepository
    {
        private static string baseDirectory =
            Path.GetDirectoryName(typeof(ReportExcelRepository).Assembly.GetName().CodeBase).Substring(6);

        public override void InitMapper()
        {
            Mapper = new DataModelMapper();
        }

        public override IWorkbook LoadWorkbook(string excelFileName)
            => WorkbookExtension.Load($"{baseDirectory}\\ExcelData\\{excelFileName}");
    }
}
