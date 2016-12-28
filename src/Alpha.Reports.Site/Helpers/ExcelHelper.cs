namespace Alpha.Reports.Site.Helpers
{
    using System;
    using System.Resources;
    using App_GlobalResources;

    public static class ExcelHelper
    {
        private static ResourceManager resourceManager;

        public static ResourceManager ResourceManager
        {
            get { return resourceManager ?? Resources.ResourceManager; }
            set { resourceManager = value; }
        }

        public static string StandardizeFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            return $"{fileName.Replace(' ', '_').Replace('/', '_')}.xls";
        }

        public static string HeaderTitleMapping(string resourceKey) 
            => ResourceManager.GetString(resourceKey);        
    }
}