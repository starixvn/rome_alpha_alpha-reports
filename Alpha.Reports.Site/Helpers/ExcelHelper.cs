namespace Alpha.Reports.Site.Helpers
{
    using System;

    public static class ExcelHelper
    {
        public static string ContentType
        {
            get
            {
                return "application/vnd.ms-excel";
            }
        }

        public static string GetFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            return $"{fileName.Replace(' ', '_')}.xls";
        }
    }
}