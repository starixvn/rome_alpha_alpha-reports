namespace Alpha.Reports.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;

    public static class SiteSettings
    {
        [ThreadStatic]
        private static NameValueCollection appSettings;

        public static bool LocalTest => GetConfigValue("LocalTest") == "1";

        public static string StoredProcedurePath => GetConfigValue("StoredProcedurePath");

        public static string ReportsAppPath => GetConfigValue("ReportsAppPath");

        internal static NameValueCollection AppSettings
        {
            get { return appSettings ?? ConfigurationManager.AppSettings; }

            set { appSettings = value; }
        }

        public static string GetConfigValue(string configName) => AppSettings[configName];

        public static void Verify()
        {
            Require(StoredProcedurePath, nameof(StoredProcedurePath));
            Require(ReportsAppPath, nameof(ReportsAppPath));
        }

        private static void Require(string key, string keyName)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ConfigurationErrorsException($"{keyName} is null or whitespace");
            }
        }

        public static string BetListAppPath => GetConfigValue("BetListsAppPath") ?? "site-betlists";

        public static bool EnabledStaticData => GetConfigValue("EnabledStaticData") == "1";

        public static IEnumerable<int> SupportedJackPotProductIds => GetConfigValue("SupportedJackPotProductIds").Split(',').Select(int.Parse);
    }
}