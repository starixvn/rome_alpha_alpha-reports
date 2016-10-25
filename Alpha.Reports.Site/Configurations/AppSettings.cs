namespace Alpha.Reports.Site.Configurations
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Web;
    using Fanex.Common;
    using Models;
    using Newtonsoft.Json;

    public class AppSettings
    {
        private static IEnumerable<Product> products;

        public static string OldMainAppPath => GetConfigValue("OldMainAppPath");

        public static string AccessDeniedPage => GetConfigValue("AccessDeniedPage");

        public static string StoreProcedurePath => GetConfigValue("StoreProcedurePath");

        public static bool IsShowKenoSetting => Converter.ToBool(GetConfigValue("IsShowKenoSetting"));

        public static string AssetAppPath => GetConfigValue("AssetAppPath");

        private static string ProductConfigPath => GetConfigValue("Reports:Products");

        public static IEnumerable<Product> Products
        {
            get
            {
                if (products == null)
                {
                    LoadProductConfig();
                }

                return products;
            }
        }

        public static string GetConfigValue(string configName) => ConfigurationManager.AppSettings[configName];

        public static void LoadProductConfig()
        {
            if (HttpContext.Current == null || HttpContext.Current.Server == null)
            {
                return;
            }

            var configFilePath = HttpContext.Current.Server.MapPath(ProductConfigPath);

            if (!File.Exists(configFilePath))
            {
                throw new FileNotFoundException("Cannot find security code json file");
            }

            using (StreamReader stream = File.OpenText(configFilePath))
            {
                products = JsonConvert.DeserializeObject<IEnumerable<Product>>(stream.ReadToEnd());
            }
        }
    }
}