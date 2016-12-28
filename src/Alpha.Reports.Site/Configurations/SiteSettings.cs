namespace Alpha.Reports.Site.Configurations
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;

    public static class SiteSettings
    {
        [ThreadStatic]
        private static NameValueCollection appSettings;

        public static string OldMainAppPath => GetConfigValue("OldMainAppPath");

        public static string AccessDeniedPage => GetConfigValue("AccessDeniedPage");

        public static string StoreProcedurePath => GetConfigValue("StoreProcedurePath");

        public static string AssetAppPath => GetConfigValue("AssetAppPath");

        public static string StaticResourcesVersion => GetConfigValue("AssetsApp:StaticResourcesVersion");

        internal static NameValueCollection AppSettings
        {
            get { return appSettings ?? ConfigurationManager.AppSettings; }

            set { appSettings = value; }
        }

        public static string GetConfigValue(string configName) => AppSettings[configName];

        public static void Verify()
        {
            CheckNullOrWhiteSpace(OldMainAppPath);
            CheckNullOrWhiteSpace(StoreProcedurePath);
            CheckNullOrWhiteSpace(AssetAppPath);
        }

        private static void CheckNullOrWhiteSpace(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new InvalidOperationException($"{nameof(path)} is null or whitespace");
            }
        }
    }
}