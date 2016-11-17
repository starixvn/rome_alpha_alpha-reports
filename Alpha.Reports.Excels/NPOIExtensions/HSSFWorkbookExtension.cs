namespace Alpha.Reports.Excels.NPOIExtensions
{
    using System.IO;
    using NPOI.HSSF.UserModel;

    public static class HSSFWorkbookExtension
    {
        public static HSSFWorkbook Load(string templatePath)
        {
            using (FileStream file = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
            {
                var workbook = new HSSFWorkbook(file);

                return workbook;
            }
        }

        public static byte[] ToBytes(this HSSFWorkbook workbook)
        {
            using (var memoryStream = new MemoryStream())
            {
                workbook.Write(memoryStream);

                return memoryStream.GetBuffer();
            }
        }
    }
}
