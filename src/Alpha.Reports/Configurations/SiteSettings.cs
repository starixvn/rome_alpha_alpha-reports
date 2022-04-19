namespace Alpha.Reports.Configurations
{
    using Sunplus.Agent.Model.Entities;
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

        public static string MessageAppPath => GetConfigValue("MessageAppPath") ?? "site-messages";

        public static bool EnabledStaticData => GetConfigValue("EnabledStaticData") == "1";

        public static string RemoteRefURL => GetConfigValue("RemoteRefURL");

        public static string ReportRouting => ConfigurationManager.AppSettings["Report:Routing"];//UserSession.Current.GetProperty<string>("Report_Routing") ?? string.Empty;

        public static IEnumerable<int> ProductOfferProductDetail
        {
            get
            {
                var productIds = GetConfigValue("ProductOfferProductDetail");
                if (!string.IsNullOrEmpty(productIds))
                {
                    return productIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(i => int.Parse(i));
                }

                return Enumerable.Empty<int>();
            }
        }

        public static byte SettingStatus
        {
            get
            {
                byte.TryParse(ConfigurationManager.AppSettings["SettingStatus"], out byte settingStatus);
                return settingStatus;
            }
        }

        public static IEnumerable<string> OfferSabaPromotionSites
            => GetConfigValue("EnableProduct-37:Sites")?.Split(new[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);

        public static IEnumerable<string> OfferSabaPromotionHeadlineSites
            => GetConfigValue("EnableProduct-37:HLSites")?.Split(new[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);

        public static string SeperateSabaSportsDate
            => GetConfigValue("SeperateSabaSportsDate");

        private static string[] allowedSitesToViewBonusDetail;
        public static string[] AllowedSitesToViewBonusDetail
        {
            get
            {
                if (allowedSitesToViewBonusDetail == null)
                {
                    var allowedSites = GetConfigValue("AllowedSitesToViewBonusDetail");

                    allowedSitesToViewBonusDetail = string.IsNullOrWhiteSpace(allowedSites)
                        ? Array.Empty<string>()
                        : allowedSites
                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim().ToUpperInvariant())
                            .ToArray();
                }

                return allowedSitesToViewBonusDetail;
            }
        }
    }
}