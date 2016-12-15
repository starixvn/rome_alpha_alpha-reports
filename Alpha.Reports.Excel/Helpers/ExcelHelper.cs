namespace Alpha.Reports.Excel.Helpers
{
    using System;
    using AppResources;

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
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            return $"{fileName.Replace(' ', '_').Replace('/', '_')}.xls";
        }

        public static string HeaderTitleMapping(string resourceKey)
        {
            return Resources.ResourceManager.GetString(resourceKey);
        }
    }
}
