namespace Alpha.Reports.Site.Configurations
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using Fanex.Common;

    public static class SiteSettings
    {
        private static NameValueCollection appSettings = ConfigurationManager.AppSettings;

        public static string OldMainAppPath => GetConfigValue("OldMainAppPath");

        public static string AccessDeniedPage => GetConfigValue("AccessDeniedPage");

        public static string StoreProcedurePath => GetConfigValue("StoreProcedurePath");

        public static bool IsShowKenoSetting => Converter.ToBool(GetConfigValue("IsShowKenoSetting"));

        public static string AssetAppPath => GetConfigValue("AssetAppPath");

        internal static NameValueCollection AppSettings
        {
            get { return appSettings; }
            set { appSettings = value; }
        }

        public static string GetConfigValue(string configName) => appSettings[configName];

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