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
            Require(OldMainAppPath, nameof(OldMainAppPath));
            Require(StoreProcedurePath, nameof(StoreProcedurePath));
            Require(AssetAppPath, nameof(AssetAppPath));                   
        }

        private static void Require(string key, string keyName)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ConfigurationErrorsException($"{keyName} is null or whitespace");
            }
        }
    }
}