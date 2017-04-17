namespace Alpha.Reports.Configurations
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using Fanex.Common;

    public static class SiteSettings
    {
        [ThreadStatic]
        private static NameValueCollection appSettings;

        public static bool LocalTest => Converter.ToBool(GetConfigValue("LocalTest"));

        public static string StoreProcedurePath => GetConfigValue("StoreProcedurePath");

        public static string ReportsAppPath => GetConfigValue("ReportsAppPath");

        internal static NameValueCollection AppSettings
        {
            get { return appSettings ?? ConfigurationManager.AppSettings; }

            set { appSettings = value; }
        }

        public static string GetConfigValue(string configName) => AppSettings[configName];

        public static void Verify()
        {
            Require(StoreProcedurePath, nameof(StoreProcedurePath));
            Require(ReportsAppPath, nameof(ReportsAppPath));
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