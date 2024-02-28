namespace Alpha.Reports.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using System.Web;

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

        public static string RemoteRefURL => GetConfigValue("RemoteRefURL");

        public static string ReportRouting
        {
            get
            {
                var context = HttpContext.Current;
                var cookieless = context.Session.IsCookieless ? $"/(S({context.Session.SessionID}))" : string.Empty;
                return IsStandaloneReport ? $"site-reports{cookieless}/" : string.Empty;
            }
        }

        public static bool IsStandaloneReport => GetConfigValue("IsStandaloneReport") == "1";

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

        public static bool IsMBCSite => !IsSiteLabel789y;

        public static bool IsSiteLabel789y => GetConfigValue("SiteLabel").Equals(GetConfigValue("SiteLabel789y"));

        public static bool DisabledVietnamese => IsMBCSite;

        public static string JsVersion => GetConfigValue("JsVersion");

        public static bool EnableLogDifferentSessionBetweenMainAndReports { get; } = "1" == GetConfigValue("EnableLogDifferentSessionBetweenMainAndReports");

        private static IDictionary<string, bool> newWinLossDetailReportOfferSites;
        public static IDictionary<string, bool> NewWinLossDetailReportOfferSites
        {
            get
            {
                if (newWinLossDetailReportOfferSites == null)
                {
                    newWinLossDetailReportOfferSites = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
                    (GetConfigValue("NewWinLossDetailReport:OfferSites") ?? string.Empty)
                        .Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList()
                        .ForEach(item => newWinLossDetailReportOfferSites[item.Trim()] = true);
                }

                return newWinLossDetailReportOfferSites;
            }
        }

        public static bool IsOfferNewWinLossDetailReport(string customerSite)
            => NewWinLossDetailReportOfferSites.ContainsKey(customerSite)
            || NewWinLossDetailReportOfferSites.ContainsKey("*");
    }
}