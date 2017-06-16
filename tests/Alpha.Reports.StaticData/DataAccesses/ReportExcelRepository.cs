using System.IO;
using Alpha.IntegrationTest.Npoi;
using Alpha.IntegrationTest.Repository;
using Alpha.Reports.StaticData.Configurations;
using NPOI.SS.UserModel;

namespace Alpha.Reports.StaticData.DataAccesses
{
    public class ReportExcelRepository : ExcelRepository
    {
        private static string baseDirectory =
            Path.GetDirectoryName(typeof(ReportExcelRepository).Assembly.GetName().CodeBase).Substring(6);

        public override void InitMapper()
        {
            Mapper = new MapperForStaticData();
        }

        public override IWorkbook LoadWorkbook(string excelFileName)
            => NpoiWorkbook.Load($"{baseDirectory}\\ExcelData\\{excelFileName}");
    }
}
