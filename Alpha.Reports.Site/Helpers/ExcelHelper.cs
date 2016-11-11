namespace Alpha.Reports.Site.Helpers
{
    public static class ExcelHelper
    {
        public static string ContentType = "application/vnd.ms-excel";

        public static string GetFileName(string fileName)
        {
            return $"{fileName.Replace(' ', '_')}.xls";
        }
    }
}