namespace Alpha.Reports.Site.Configurations
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Web;
    using Models;
    using Newtonsoft.Json;

    public class AppSettings
    {
        private static IEnumerable<Product> products;

        public static string OldMainAppPath
        {
            get
            {
                return GetConfigValue("OldMainAppPath");
            }
        }

        public static string AccessDeniedPage
        {
            get
            {
                return GetConfigValue("AccessDeniedPage");
            }
        }

        public static string StoreProcedurePath
        {
            get
            {
                return GetConfigValue("StoreProcedurePath");
            }
        }

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

        public static string AssetAppPath
        {
            get
            {
                return GetConfigValue("AssetAppPath");
            }
        }

        private static string ProductConfigPath
        {
            get
            {
                return GetConfigValue("Reports:Products");
            }
        }

        public static string GetConfigValue(string configName)
        {
            return ConfigurationManager.AppSettings[configName];
        }

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