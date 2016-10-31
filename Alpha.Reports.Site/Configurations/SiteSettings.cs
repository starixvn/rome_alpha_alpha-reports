namespace Alpha.Reports.Site.Configurations
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Web;
    using Fanex.Common;
    using Models;
    using Newtonsoft.Json;

    public static class SiteSettings
    {
        private static IEnumerable<Product> products;
        private static IEnumerable<ReportRange> reportRanges;

        public static string OldMainAppPath => GetConfigValue("OldMainAppPath");

        public static string ReportsAppPath => GetConfigValue("ReportsAppPath");

        public static string AccessDeniedPage => GetConfigValue("AccessDeniedPage");

        public static string StoreProcedurePath => GetConfigValue("StoreProcedurePath");

        public static bool IsShowKenoSetting => Converter.ToBool(GetConfigValue("IsShowKenoSetting"));

        public static string AssetAppPath => GetConfigValue("AssetAppPath");

        private static string ProductConfigPath => GetConfigValue("Reports:Products");

        private static string ReportRangeConfigPath => GetConfigValue("Reports:ReportRanges");

        public static string VirtualDirectory => GetConfigValue("VirtualDirectory");

        public static IEnumerable<Product> Products => products;

        public static IEnumerable<ReportRange> ReportRanges => reportRanges;

        public static string GetConfigValue(string configName) => ConfigurationManager.AppSettings[configName];

        public static void LoadSettings()
        {
            products = LoadConfig<IEnumerable<Product>>(ProductConfigPath);
            reportRanges = LoadConfig<IEnumerable<ReportRange>>(ReportRangeConfigPath);
        }

        public static T LoadConfig<T>(string configPath)
        {
            if (HttpContext.Current?.Server == null)
            {
                return default(T);
            }

            var configFilePath = HttpContext.Current.Server.MapPath(configPath);

            if (!File.Exists(configFilePath))
            {
                throw new FileNotFoundException("Cannot find json config file: " + configPath);
            }

            T result;

            using (StreamReader stream = File.OpenText(configFilePath))
            {
                result = JsonConvert.DeserializeObject<T>(stream.ReadToEnd());
            }

            return result;
        }
    }
}