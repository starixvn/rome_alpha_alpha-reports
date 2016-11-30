namespace Alpha.Reports.Site.Configurations
{
    using System;
    using System.Configuration;
    using Fanex.Common;

    public static class SiteSettings
    {
        public static string OldMainAppPath => GetConfigValue("OldMainAppPath");

        public static string AccessDeniedPage => GetConfigValue("AccessDeniedPage");

        public static string StoreProcedurePath => GetConfigValue("StoreProcedurePath");

        public static bool IsShowKenoSetting => Converter.ToBool(GetConfigValue("IsShowKenoSetting"));

        public static string AssetAppPath => GetConfigValue("AssetAppPath");

        public static string GetConfigValue(string configName) => ConfigurationManager.AppSettings[configName];

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